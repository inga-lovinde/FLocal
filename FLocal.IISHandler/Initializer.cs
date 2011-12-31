using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Web.Core;
using Web.Core.DB;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler {
	class Initializer {

		public static readonly Initializer instance = new Initializer();

		private bool isInitialized;

		private readonly object locker = new object();

		/// <summary>
		/// We wouldn't want class constructor to throw an exception, so all "smart" things are inside the Initialize();
		/// </summary>
		private Initializer() {
			this.isInitialized = false;
		}

		public void Initialize() {
			if(!this.isInitialized) {
				lock(this.locker) {
					if(!this.isInitialized) {
						this.DoInitialize();
						this.isInitialized = true;
					}
				}
			}
		}

		private void DoInitialize() {
			Config.Init(ConfigurationManager.AppSettings);
			foreach(var cacher in this.cachers) {
				System.Threading.ThreadPool.QueueUserWorkItem(state => this.WrapCacher(cacher));
			}
		}

		private IEnumerable<Action> cachers {
			get {
				yield return this.GetTableCacher<LocalNetwork>(LocalNetwork.LoadByIds, LocalNetwork.TableSpec.instance);
				yield return this.GetTableCacher<Machichara>(Machichara.LoadByIds, Machichara.TableSpec.instance);
				yield return this.GetTableCacher<ModernSkin>(ModernSkin.LoadByIds, ModernSkin.TableSpec.instance);
				yield return this.GetTableCacher<PostLayer>(PostLayer.LoadByIds, PostLayer.TableSpec.instance);
				yield return this.GetTableCacher<PunishmentType>(PunishmentType.LoadByIds, PunishmentType.TableSpec.instance);
				yield return this.GetTableCacher<QuickLink>(QuickLink.LoadByIds, QuickLink.TableSpec.instance);
				yield return this.GetTableCacher<Skin>(Skin.LoadByIds, Skin.TableSpec.instance);
				yield return this.GetTableCacher<UserGroup>(UserGroup.LoadByIds, UserGroup.TableSpec.instance);
				yield return this.CacheCategories;
			}
		}

		private void WrapCacher(Action cacher) {
			try {
				cacher();
			} catch(FLocalException) {
			}
		}

		private Action GetTableCacher<SqlObjectType>(Func<IEnumerable<int>, List<SqlObjectType>> objectsLoader, ITableSpec tableSpec)
			where SqlObjectType : SqlObject<SqlObjectType>, new()
		{
			return () => {
				foreach(
					SqlObjectType sqlObject
					in
					objectsLoader(
						from stringId
						in Config.instance.mainConnection.LoadIdsByConditions(
							tableSpec,
							new Web.Core.DB.conditions.EmptyCondition(),
							Diapasone.unlimited
						)
						select int.Parse(stringId)
					)
				) {
					sqlObject.LoadIfNotLoaded();
				}
			};
		}

		private void CacheCategories() {
			foreach(var category in Category.allCategories) {
				category.LoadIfNotLoaded();
				foreach(var board in category.subBoards) {
					this.CacheBoard(board);
				}
			}

			int? _maxPostId = (
					from category in Category.allCategories
					from board in category.subBoards
					select board.lastPostId
				).Max();

			if(_maxPostId.HasValue) {
				int maxPostId = _maxPostId.Value;
				int minPostId = Math.Min(maxPostId - 10000, 0);
				for(int i=maxPostId; i>minPostId; i++) {
					try {
						var post = Post.LoadById(i);
					} catch(NotFoundInDBException) {
					}
				}
			}

		}

		private void CachePost(Post post) {
			post.LoadIfNotLoaded();
			post.thread.LoadIfNotLoaded();
			post.poster.LoadIfNotLoaded();
			post.latestRevision.LoadIfNotLoaded();
			post.parentPost.LoadIfNotLoaded();
			post.parentPost.latestRevision.LoadIfNotLoaded();
			foreach(var punishment in post.punishments) {
				punishment.LoadIfNotLoaded();
				punishment.moderator.LoadIfNotLoaded();
			}
			if(post.id > post.thread.firstPostId) {
				CachePost(post.thread.firstPost);
			}
		}

		private void CacheBoard(Board board) {
			board.LoadIfNotLoaded();
			foreach(var subBoard in board.subBoards) {
				CacheBoard(subBoard);
			}
		}

	}
}
