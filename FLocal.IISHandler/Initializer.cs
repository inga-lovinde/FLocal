using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Web.Core;
using Web.Core.DB;
using FLocal.Common;
using FLocal.Common.dataobjects;
using System.IO;
using System.Threading;
using FLocal.Patcher.Common;

namespace FLocal.IISHandler {
	class Initializer {

		public static readonly Initializer instance = new Initializer();

		private bool isInitialized;
		private bool isCached;

		private readonly object locker = new object();

		/// <summary>
		/// We wouldn't want class constructor to throw an exception, so all "smart" things are inside the Initialize();
		/// </summary>
		private Initializer() {
			this.isInitialized = false;
			this.isCached = false;
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

			if(!this.isCached) {
				lock(this.locker) {
					if(!this.isCached) {
						this.DoCache();
						this.isCached = true;
					}
				}
			}
		}

		private void DoInitialize() {
			Config.Init(ConfigurationManager.AppSettings);
			PatcherConfiguration.Init(ConfigurationManager.AppSettings);
		}

		private void DoCache() {
			if(!PatcherInfo.instance.IsNeedsPatching) {
				string dir = FLocal.Common.Config.instance.dataDir + "Logs\\";
				using(StreamWriter writer = new StreamWriter(dir + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".INITIALIZE.txt")) {
					writer.WriteLine("###INITIALIZE###");
					foreach(var cacher in this.cachers) {
						System.Threading.ThreadPool.QueueUserWorkItem(this.GetCacheWrapper(cacher));
						writer.WriteLine("Pending " + cacher.Key);
					}
				}
			}
		}

		private IEnumerable<KeyValuePair<string, Action>> cachers {
			get {
				yield return this.GetTableCacher<Machichara>(Machichara.LoadByIds, Machichara.TableSpec.instance);
				yield return this.GetTableCacher<ModernSkin>(ModernSkin.LoadByIds, ModernSkin.TableSpec.instance);
				yield return this.GetTableCacher<PostLayer>(PostLayer.LoadByIds, PostLayer.TableSpec.instance);
				yield return this.GetTableCacher<PunishmentType>(PunishmentType.LoadByIds, PunishmentType.TableSpec.instance);
				yield return this.GetTableCacher<QuickLink>(QuickLink.LoadByIds, QuickLink.TableSpec.instance);
				yield return this.GetTableCacher<Skin>(Skin.LoadByIds, Skin.TableSpec.instance);
				yield return this.GetTableCacher<UserGroup>(UserGroup.LoadByIds, UserGroup.TableSpec.instance);
				yield return new KeyValuePair<string, Action>("categories", this.CacheCategories);
			}
		}

		private WaitCallback GetCacheWrapper(KeyValuePair<string, Action> cacher) {
			return state => {
				string dir = FLocal.Common.Config.instance.dataDir + "Logs\\";

				DateTime start = DateTime.Now;
				/*using(StreamWriter writer = new StreamWriter(dir + start.ToString("yyyy-MM-dd_HH-mm-ss") + ".init." + cacher.Key + ".txt")) {
					writer.WriteLine("###INITIALIZER/CACHER###");
					writer.WriteLine("info: " + cacher.Value.Method.Name);
					writer.WriteLine();
				}*/

				FLocalException error = null;
				try {
					cacher.Value();
				} catch(FLocalException e) {
					error = e;
				}

				DateTime end = DateTime.Now;
				using(StreamWriter writer = new StreamWriter(dir + end.ToString("yyyy-MM-dd_HH-mm-ss") + ".initend." + cacher.Key + ".txt")) {
					writer.WriteLine("###INITIALIZER/CACHER###");
					writer.WriteLine("info: " + cacher.Value.Method.Name);
					writer.WriteLine("Start: " + start);
					writer.WriteLine();
					if(error != null) {
						writer.WriteLine("Exception: " + error.GetType().FullName);
						writer.WriteLine("Guid: " + error.GetGuid().ToString());
						writer.WriteLine(error.Message);
						if(error is FLocalException) {
							writer.WriteLine(((FLocalException)error).FullStackTrace);
						} else {
							writer.WriteLine(error.StackTrace);
						}
					}
				}
			};
		}

		private KeyValuePair<string, Action> GetTableCacher<SqlObjectType>(Func<IEnumerable<int>, List<SqlObjectType>> objectsLoader, ITableSpec tableSpec)
			where SqlObjectType : SqlObject<SqlObjectType>, new()
		{
			return new KeyValuePair<string,Action>(
				typeof(SqlObjectType).FullName,
				() => {
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
				}
			);
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
				int minPostId = Math.Max(maxPostId - 10000, 0);
				for(int i=maxPostId; i>minPostId; i--) {
					try {
						CachePost(Post.LoadById(i));
					} catch(NotFoundInDBException) {
					}
				}
			}

		}

		private void CachePost(Post post) {
			post.LoadIfNotLoaded();
			post.thread.LoadIfNotLoaded();
			post.poster.LoadIfNotLoaded();
			if(post.poster.avatarId != null) {
				post.poster.avatar.LoadIfNotLoaded();
			}
			if(post.revision != null) {
				post.latestRevision.LoadIfNotLoaded();
			}
			if(post.parentPostId != null) {
				post.parentPost.LoadIfNotLoaded();
				if(post.parentPost.revision != null) {
					post.parentPost.latestRevision.LoadIfNotLoaded();
				}
			}
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
