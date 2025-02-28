﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class Board : SqlObject<Board> {

		[Flags]
		public enum SubboardsOptions {
			None = 0x0,
			FirstLevel = 0x1,
			AllLevels = 0x3
		}

		public class TableSpec : IComplexSqlObjectTableSpec {
			public const string TABLE = "Boards";
			public const string FIELD_ID = "Id";
			public const string FIELD_SORTORDER = "SortOrder";
			public const string FIELD_CATEGORYID = "CategoryId";
			public const string FIELD_LASTPOSTID = "LastPostId";
			public const string FIELD_TOTALPOSTS = "TotalPosts";
			public const string FIELD_TOTALTHREADS = "TotalThreads";
			public const string FIELD_NAME = "Name";
			public const string FIELD_DESCRIPTION = "Comment";
			public const string FIELD_PARENTBOARDID = "ParentBoardId";
			public const string FIELD_LEGACYNAME = "LegacyName";
			public const string FIELD_ISTRANSFERTARGET = "IsTransferTarget";
			public const string FIELD_ISTOPICSTARTERMODERATION = "IsTopicstarterModeration";
			public const string FIELD_ISFLOOBAGE = "IsFloobage";
			public const string FIELD_ADMINISTRATORID = "AdministratorId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
			public void refreshSqlObjectAndRelated(int id) {
				Refresh(id);
				LoadById(id).subBoards_Reset();
			}
		}

		public class ReadMarkerTableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Boards_ReadMarkers";
			public const string FIELD_ID = "Id";
			public const string FIELD_BOARDID = "BoardId";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_LASTREADDATE = "LastReadDate";
			public static readonly ReadMarkerTableSpec instance = new ReadMarkerTableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) {  }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _sortOrder;
		public int sortOrder {
			get {
				this.LoadIfNotLoaded();
				return this._sortOrder;
			}
		}
		
		private int? _categoryId;
		public int? categoryId {
			get {
				this.LoadIfNotLoaded();
				return this._categoryId;
			}
		}
		public Category category {
			get {
				return Category.LoadById(this.categoryId.Value);
			}
		}

		private int? _lastPostId;
		public int? lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId;
			}
		}
		public Post lastPost {
			get {
				return Post.LoadById(this.lastPostId.Value);
			}
		}

		private int _totalPosts;
		public int totalPosts {
			get {
				this.LoadIfNotLoaded();
				return this._totalPosts;
			}
		}

		private int _totalThreads;
		public int totalThreads {
			get {
				this.LoadIfNotLoaded();
				return this._totalThreads;
			}
		}

		public int totalThreadsExcludingSubboards {
			get {
				return this.totalThreads - this.subBoards.Sum(board => board.totalThreads);
			}
		}

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}
		public string nameTranslit {
			get {
				return TranslitManager.Translit(this.name);
			}
		}

		private string _description;
		public string description {
			get {
				this.LoadIfNotLoaded();
				return this._description;
			}
		}

		private int? _parentBoardId;
		public int? parentBoardId {
			get {
				this.LoadIfNotLoaded();
				return this._parentBoardId;
			}
		}
		public Board parentBoard {
			get {
				return Board.LoadById(this.parentBoardId.Value);
			}
		}

		private string _legacyName;
		public string legacyName {
			get {
				this.LoadIfNotLoaded();
				return this._legacyName;
			}
		}
		
		private bool _isTransferTarget;
		public bool isTransferTarget {
			get {
				this.LoadIfNotLoaded();
				return this._isTransferTarget;
			}
		}

		private bool _isTopicstarterModeration;
		public bool isTopicstarterModeration {
			get {
				this.LoadIfNotLoaded();
				return this._isTopicstarterModeration;
			}
		}

		private bool _isFloobage;
		public bool isFloobage {
			get {
				this.LoadIfNotLoaded();
				return this._isFloobage;
			}
		}

		private int _administratorId;
		public int administratorId {
			get {
				this.LoadIfNotLoaded();
				return this._administratorId;
			}
		}
		public Account administrator {
			get {
				return Account.LoadById(this._administratorId);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._sortOrder = int.Parse(data[TableSpec.FIELD_SORTORDER]);
			this._categoryId = Util.ParseInt(data[TableSpec.FIELD_CATEGORYID]);
			this._lastPostId = Util.ParseInt(data[TableSpec.FIELD_LASTPOSTID]);
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._totalThreads = int.Parse(data[TableSpec.FIELD_TOTALTHREADS]);
			this._name = data[TableSpec.FIELD_NAME];
			this._description = data[TableSpec.FIELD_DESCRIPTION];
			this._parentBoardId = Util.ParseInt(data[TableSpec.FIELD_PARENTBOARDID]);
			this._legacyName = data[TableSpec.FIELD_LEGACYNAME].ToLower();
			this._isTransferTarget = Util.string2bool(data[TableSpec.FIELD_ISTRANSFERTARGET]);
			this._isTopicstarterModeration = Util.string2bool(data[TableSpec.FIELD_ISTOPICSTARTERMODERATION]);
			this._isFloobage = Util.string2bool(data[TableSpec.FIELD_ISFLOOBAGE]);
			this._administratorId = int.Parse(data[TableSpec.FIELD_ADMINISTRATORID]);
		}

		private readonly object subBoards_Locker = new object();
		public IEnumerable<Board> subBoards {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						this.subBoards_Locker,
						() => {
							IEnumerable<int> ids = (from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new Web.Core.DB.conditions.ComparisonCondition(
									TableSpec.instance.getColumnSpec(Board.TableSpec.FIELD_PARENTBOARDID),
									Web.Core.DB.conditions.ComparisonType.EQUAL,
									this.id.ToString()
								),
								Diapasone.unlimited
							) select int.Parse(stringId)).ToList();
							Board.LoadByIds(ids);
							return ids;
						}
					)
					let board = Board.LoadById(id)
					where board.sortOrder >= 0
					orderby board.sortOrder, board.id
					select board;
			}
		}
		internal void subBoards_Reset() {
			Cache<IEnumerable<int>>.instance.delete(this.subBoards_Locker);
		}

		public bool hasNewPosts(Account account) {
			if(!this.lastPostId.HasValue) {
				return false;
			} else {
				return this.getLastReadDate(account) < this.lastPost.postDate;
			}
		}

		private XElement exportLastPostInfo(UserContext context) {
			if(!this.lastPostId.HasValue) {
				return new XElement("none");
			} else {
				return this.lastPost.exportToXml(context);
			}
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("board",
				new XElement("id", this.id),
				new XElement("name", this.name),
				new XElement("nameTranslit", this.nameTranslit),
				new XElement("description", this.description),
				new XElement("parent", this.parentBoardId.HasValue ? this.parentBoard.exportToXmlSimpleWithParent(context) : this.category.exportToXmlSimple(context))
			);
		}

		public XElement exportToXmlSimple(UserContext context, Board.SubboardsOptions subboardsOptions) {
			XElement result = new XElement("board",
				new XElement("id", this.id),
				new XElement("name", this.name),
				new XElement("description", this.description),
				new XElement("isTopicstarterModeration", this.isTopicstarterModeration.ToPlainString()),
				new XElement("isTransferTarget", this.isTransferTarget.ToPlainString())
			);
			if((subboardsOptions & SubboardsOptions.FirstLevel) == SubboardsOptions.FirstLevel) {
				result.Add(new XElement("subBoards", from board in this.subBoards select board.exportToXmlSimple(context, (subboardsOptions == SubboardsOptions.AllLevels) ? SubboardsOptions.AllLevels : SubboardsOptions.None)));
			}
			return result;
		}

		public XElement exportToXml(UserContext context, Board.SubboardsOptions subboardsOptions, params XElement[] additional) {
			XElement result = new XElement("board",
				new XElement("id", this.id),
				new XElement("sortOrder", this.sortOrder),
				new XElement("categoryId", this.categoryId),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalThreads", this.totalThreads),
				new XElement("name", this.name),
				new XElement("description", this.description),
				new XElement("lastPostInfo", this.exportLastPostInfo(context)),
				new XElement("moderators", from moderator in Moderator.GetModerators(this) select moderator.user.exportToXmlForViewing(context)),
				new XElement("isTopicstarterModeration", this.isTopicstarterModeration.ToPlainString()),
				new XElement("isTransferTarget", this.isTransferTarget.ToPlainString()),
				new XElement("administrator", this.administrator.user.exportToXmlForViewing(context, new XElement("isAdministrator", "true")))
			);

			if(context.account != null) {
				result.Add(new XElement("hasNewPosts", this.hasNewPosts(context.account).ToPlainString()));
			}

			if((subboardsOptions & SubboardsOptions.FirstLevel) == SubboardsOptions.FirstLevel) {
				result.Add(new XElement("subBoards",
					from board in this.subBoards select board.exportToXml(context, (subboardsOptions == SubboardsOptions.AllLevels) ? SubboardsOptions.AllLevels : SubboardsOptions.None)
				));
			}

			if(additional.Length > 0) {
				result.Add(additional);
			}

			return result;
		}

		private static readonly IEnumerable<int> allBoardsIds = from stringId in Config.instance.mainConnection.LoadIdsByConditions(TableSpec.instance, new EmptyCondition(), Diapasone.unlimited) select int.Parse(stringId);

		private static Dictionary<string, int> legacyName2Id = new Dictionary<string,int>();
		public static Board LoadByLegacyName(string _legacy) {
			if((_legacy == null) || (_legacy == "")) throw new FLocalException("legacy name is empty");
			string legacy = _legacy.ToLower();
			if(!legacyName2Id.ContainsKey(legacy)) {
				lock(legacyName2Id) {
					if(!legacyName2Id.ContainsKey(legacy)) {
						legacyName2Id[legacy] = (from boardId in allBoardsIds let board = Board.LoadById(boardId) where board.legacyName == legacy select boardId).Single();
					}
				}
			}
			return Board.LoadById(legacyName2Id[legacy]);
		}

		public IEnumerable<Thread> getThreads(Diapasone diapasone, SortSpec[] sortBy) {
			diapasone.total = this.totalThreadsExcludingSubboards;
			return Thread.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Thread.TableSpec.instance,
					new ComparisonCondition(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_BOARDID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					diapasone,
					new JoinSpec[0],
					sortBy
				) select int.Parse(stringId)
			);
		}

		public IEnumerable<Thread> getThreads(Diapasone diapasone, bool isAscending) {
			diapasone.total = this.totalThreadsExcludingSubboards;
			return this.getThreads(
				diapasone,
				new SortSpec[] {
					new SortSpec(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_ISANNOUNCEMENT),
						false
					),
					new SortSpec(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_LASTPOSTID),
						isAscending
					),
				}
			);
		}

		public DateTime getLastReadDate(Account account) {
			List<string> stringIds = Config.instance.mainConnection.LoadIdsByConditions(
				ReadMarkerTableSpec.instance,
				new ComplexCondition(
					ConditionsJoinType.AND,
					new ComparisonCondition(
						ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_BOARDID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					new ComparisonCondition(
						ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_ACCOUNTID),
						ComparisonType.EQUAL,
						account.id.ToString()
					)
				),
				Diapasone.unlimited
			);
			if(stringIds.Count > 1) {
				throw new CriticalException("more than one row");
			}
			if(stringIds.Count < 1) {
				return new DateTime(0);
			}
			Dictionary<string, string> data = Config.instance.mainConnection.LoadById(ReadMarkerTableSpec.instance, stringIds[0]);
			return Util.ParseDateTimeFromTimestamp(data[ReadMarkerTableSpec.FIELD_LASTREADDATE]).Value;
		}

		public void markAsRead(Account account) {
			if(this.lastPostId.HasValue) {
				ChangeSetUtil.ApplyChanges(
					new InsertOrUpdateChange(
						ReadMarkerTableSpec.instance,
						new Dictionary<string,AbstractFieldValue> {
							{ ReadMarkerTableSpec.FIELD_BOARDID, new ScalarFieldValue(this.id.ToString()) },
							{ ReadMarkerTableSpec.FIELD_ACCOUNTID, new ScalarFieldValue(account.id.ToString()) },
							{ ReadMarkerTableSpec.FIELD_LASTREADDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
						},
						new Dictionary<string,AbstractFieldValue> {
							{ ReadMarkerTableSpec.FIELD_LASTREADDATE, new ScalarFieldValue(DateTime.Now.ToUTCString()) },
						},
						new ComplexCondition(
							ConditionsJoinType.AND,
							new ComparisonCondition(
								ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_BOARDID),
								ComparisonType.EQUAL,
								this.id.ToString()
							),
							new ComparisonCondition(
								ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_ACCOUNTID),
								ComparisonType.EQUAL,
								account.id.ToString()
							)
						)
					)
				);
			}
		}

		public Thread CreateThread(User poster, string title, string body, PostLayer desiredLayer) {
			return this.CreateThread(poster, title, body, desiredLayer, DateTime.Now, null);
		}
		public Thread CreateThread(User poster, string title, string body, PostLayer desiredLayer, DateTime date, int? forcedPostId) {

			PostLayer actualLayer = poster.getActualLayer(this, desiredLayer);
			AbstractChange threadInsert = new InsertChange(
				Thread.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ Thread.TableSpec.FIELD_BOARDID, new ScalarFieldValue(this.id.ToString()) },
					{ Thread.TableSpec.FIELD_ISANNOUNCEMENT, new ScalarFieldValue("0") },
					{ Thread.TableSpec.FIELD_ISLOCKED, new ScalarFieldValue("0") },
					{ Thread.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
					{ Thread.TableSpec.FIELD_TOPICSTARTERID, new ScalarFieldValue(poster.id.ToString()) },
					{ Thread.TableSpec.FIELD_TOTALPOSTS, new ScalarFieldValue("0") },
					{ Thread.TableSpec.FIELD_TOTALVIEWS, new ScalarFieldValue("0") },
					{ Thread.TableSpec.FIELD_FIRSTPOSTID, new ScalarFieldValue(null) },
					{ Thread.TableSpec.FIELD_LASTPOSTID, new ScalarFieldValue(null) },
					{ Thread.TableSpec.FIELD_LASTPOSTDATE, new ScalarFieldValue(date.ToUTCString()) },
				}
			);

			using(ChangeSet threadInsertSet = new ChangeSet(), dataInsertSet = new ChangeSet()) {
				Config.Transactional(transaction => {
					threadInsertSet.Add(threadInsert);
					threadInsertSet.Apply(transaction);
					foreach(AbstractChange change in Thread.getNewPostChanges(this, threadInsert.getId().Value, null, poster, actualLayer, title, body, date, forcedPostId).Value) {
						dataInsertSet.Add(change);
					}
					dataInsertSet.Apply(transaction);
				});
			}

			return Thread.LoadById(threadInsert.getId().Value);
		}

		public readonly object locker = new object();

		public void Synchronized(Action action) {
			lock(this.locker) {
				if(this.parentBoardId.HasValue) {
					this.parentBoard.Synchronized(action);
				} else {
					action();
				}
			}
		}

		public IEnumerable<Board> boardAndParents {
			get {
				return this.ToSequence(board => board.parentBoardId.HasValue ? new Board[] { board.parentBoard } : new Board[0]);
			}
		}

		public XElement exportLayersInfoForUser(UserContext context) {
			Dictionary<int, DateTime> restrictionData = new Dictionary<int,DateTime>();
			if(context.account != null) {
				restrictionData = Restriction.GetRestrictionData(context.account.user, this);
			}
			return new XElement("layers",
				from layer in PostLayer.allLayers
				select layer.exportToXml(
					context,
					new XElement("isRestricted",
						(restrictionData.ContainsKey(layer.id) && restrictionData[layer.id].CompareTo(DateTime.Now) >= 0).ToPlainString()
					),
					new XElement("restrictionExpires",
						restrictionData.ContainsKey(layer.id) ? restrictionData[layer.id].ToXml() : null
					)
				)
			);
		}

	}
}
