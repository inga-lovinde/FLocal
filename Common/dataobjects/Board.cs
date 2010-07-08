using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class Board : SqlObject<Board> {

		public class TableSpec : ISqlObjectTableSpec {
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
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
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

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
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
		}

		private readonly object subBoards_Locker = new object();
		public IEnumerable<Board> subBoards {
			get {
				return
					from id in Cache<IEnumerable<int>>.instance.get(
						this.subBoards_Locker,
						() => {
							IEnumerable<int> ids = from stringId in Config.instance.mainConnection.LoadIdsByConditions(
								TableSpec.instance,
								new FLocal.Core.DB.conditions.ComparisonCondition(
									TableSpec.instance.getColumnSpec(Board.TableSpec.FIELD_PARENTBOARDID),
									FLocal.Core.DB.conditions.ComparisonType.EQUAL,
									this.id.ToString()
								),
								Diapasone.unlimited
							) select int.Parse(stringId);
							Board.LoadByIds(ids);
							return ids;
						}
					)
					let board = Board.LoadById(id)
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
				return this.lastPost.exportToXmlWithoutThread(context, false);
			}
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("board",
				new XElement("id", this.id),
				new XElement("name", this.name),
				new XElement("description", this.description),
				new XElement("parent", this.parentBoardId.HasValue ? this.parentBoard.exportToXmlSimpleWithParent(context) : this.category.exportToXmlSimple(context))
			);
		}

		public XElement exportToXml(UserContext context, bool includeSubBoards, params XElement[] additional) {
			XElement result = new XElement("board",
				new XElement("id", this.id),
				new XElement("sortOrder", this.sortOrder),
				new XElement("categoryId", this.categoryId),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalThreads", this.totalThreads),
				new XElement("name", this.name),
				new XElement("description", this.description),
				new XElement("lastPostInfo", this.exportLastPostInfo(context))
			);

			if(context.account != null) {
				result.Add(new XElement("hasNewPosts", this.hasNewPosts(context.account).ToPlainString()));
			}

			if(includeSubBoards) {
				result.Add(new XElement("subBoards",
					from board in this.subBoards select board.exportToXml(context, false)
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

		public IEnumerable<Thread> getThreads(Diapasone diapasone) {
			return this.getThreads(
				diapasone,
				new SortSpec[] {
					new SortSpec(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_ISANNOUNCEMENT),
						false
					),
					new SortSpec(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_LASTPOSTID),
						false
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

	}
}
