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
	public class Thread : SqlObject<Thread> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Threads";
			public const string FIELD_ID = "Id";
			public const string FIELD_BOARDID = "BoardId";
			public const string FIELD_FIRSTPOSTID = "FirstPostId";
			public const string FIELD_TOPICSTARTERID = "TopicstarterId";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_LASTPOSTID = "LastPostId";
			public const string FIELD_LASTPOSTDATE = "LastPostDate";
			public const string FIELD_ISANNOUNCEMENT = "IsAnnouncement";
			public const string FIELD_ISLOCKED = "IsLocked";
			public const string FIELD_TOTALPOSTS = "TotalPosts";
			public const string FIELD_TOTALVIEWS = "TotalViews";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		public class ReadMarkerTableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Threads_ReadMarkers";
			public const string FIELD_ID = "Id";
			public const string FIELD_THREADID = "ThreadId";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_POSTID = "PostId";
			public static readonly ReadMarkerTableSpec instance = new ReadMarkerTableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) {  }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _boardId;
		public int boardId {
			get {
				this.LoadIfNotLoaded();
				return this._boardId;
			}
		}
		public Board board {
			get {
				return Board.LoadById(this.boardId);
			}
		}

		private int _firstPostId;
		public int firstPostId {
			get {
				this.LoadIfNotLoaded();
				return this._firstPostId;
			}
		}
		public Post firstPost {
			get {
				return Post.LoadById(this.firstPostId);
			}
		}

		private int _topicstarterId;
		public int topicstarterId {
			get {
				this.LoadIfNotLoaded();
				return this._topicstarterId;
			}
		}
		public User topicstarter {
			get {
				return User.LoadById(this.topicstarterId);
			}
		}

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}
		
		private int _lastPostId;
		public int lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId;
			}
		}

		private DateTime _lastPostDate;
		public DateTime lastPostDate {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostDate;
			}
		}

		private bool _isAnnouncement;
		public bool isAnnouncement {
			get {
				this.LoadIfNotLoaded();
				return this._isAnnouncement;
			}
		}

		private bool _isLocked;
		public bool isLocked {
			get {
				this.LoadIfNotLoaded();
				return this._isAnnouncement;
			}
		}

		private int _totalPosts;
		public int totalPosts {
			get {
				this.LoadIfNotLoaded();
				return this._totalPosts;
			}
		}

		private int _totalViews;
		public int totalViews {
			get {
				this.LoadIfNotLoaded();
				return this._totalViews;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._boardId = int.Parse(data[TableSpec.FIELD_BOARDID]);
			this._firstPostId = int.Parse(data[TableSpec.FIELD_FIRSTPOSTID]);
			this._topicstarterId = int.Parse(data[TableSpec.FIELD_TOPICSTARTERID]);
			this._title = data[TableSpec.FIELD_TITLE];
			this._lastPostId = int.Parse(data[TableSpec.FIELD_LASTPOSTID]);
			this._lastPostDate = new DateTime(long.Parse(data[TableSpec.FIELD_LASTPOSTDATE]));
			this._isAnnouncement = FLocal.Core.Util.string2bool(data[TableSpec.FIELD_ISANNOUNCEMENT]);
			this._isLocked = FLocal.Core.Util.string2bool(data[TableSpec.FIELD_ISLOCKED]);
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._totalViews = int.Parse(data[TableSpec.FIELD_TOTALVIEWS]);
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("thread",
				new XElement("id", this.id),
				new XElement("name", this.title),
				new XElement("parent", this.board.exportToXmlSimpleWithParent(context))
			);
		}

		public XElement exportToXml(UserContext context, bool includeFirstPost, params XElement[] additional) {
			XElement result = new XElement("thread",
				new XElement("id", this.id),
				new XElement("firstPostId", this.firstPostId),
				new XElement("topicstarter", this.topicstarter.exportToXmlForViewing(context)),
				new XElement("title", this.title),
				new XElement("lastPostId", this.lastPostId),
				new XElement("lastPostDate", this.lastPostDate.ToXml()),
				new XElement("isAnnouncement", this.isAnnouncement),
				new XElement("isLocked", this.isLocked),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalViews", this.totalViews),
				new XElement("bodyShort", this.firstPost.bodyShort),
				context.formatTotalPosts(this.totalPosts)
			);
			if(includeFirstPost) {
				result.Add(new XElement("firstPost", this.firstPost.exportToXmlWithoutThread(context, false)));
			}
			if(context.account != null) {
				result.Add(new XElement("afterLastRead", this.getLastReadId(context.account) + 1));
			}
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		public IEnumerable<Post> getPosts(Diapasone diapasone, UserContext context) {
			return Post.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Post.TableSpec.instance,
					new ComparisonCondition(
						Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_THREADID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Post.TableSpec.instance.getIdSpec(),
							true
						),
					}
				) select int.Parse(stringId)
			);
		}

		public void incrementViewsCounter() {
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue>() {
						{
							TableSpec.FIELD_TOTALVIEWS,
							new IncrementFieldValue()
						}
					},
					this.id
				)
			});
		}

		public int getLastReadId(Account account) {
			List<string> stringIds = Config.instance.mainConnection.LoadIdsByConditions(
				ReadMarkerTableSpec.instance,
				new ComplexCondition(
					ConditionsJoinType.AND,
					new ComparisonCondition(
						ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_THREADID),
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
				return 0;
			}
			Dictionary<string, string> data = Config.instance.mainConnection.LoadById(ReadMarkerTableSpec.instance, stringIds[0]);
			if((data[ReadMarkerTableSpec.FIELD_POSTID] == "") || (data[ReadMarkerTableSpec.FIELD_POSTID] == null)) {
				return 0;
			}
			return int.Parse(data[ReadMarkerTableSpec.FIELD_POSTID]);
		}

		public void markAsRead(Account account, Post minPost, Post maxPost) {
			ChangeSetUtil.ApplyChanges(new AbstractChange[] {
				new InsertOrUpdateChange(
					ReadMarkerTableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{
							ReadMarkerTableSpec.FIELD_THREADID,
							new ScalarFieldValue(this.id.ToString())
						},
						{
							ReadMarkerTableSpec.FIELD_ACCOUNTID,
							new ScalarFieldValue(account.id.ToString())
						},
						{
							ReadMarkerTableSpec.FIELD_POSTID,
							new ScalarFieldValue(
								(minPost.id <= this.firstPostId)
								?
								maxPost.id.ToString()
								:
								null
							) },
					},
					new Dictionary<string,AbstractFieldValue> {
						{
							ReadMarkerTableSpec.FIELD_POSTID,
							new IncrementFieldValue(
								s => {
									if((s == null) || (s == "")) {
										s = "0"; //workaround
									}
									if(maxPost.id < int.Parse(s)) {
										return (s == "0") ? null : s; //if some newer posts were already read
									}
									long count = Config.instance.mainConnection.GetCountByConditions(
										Post.TableSpec.instance,
										new ComplexCondition(
											ConditionsJoinType.AND,
											new ComparisonCondition(
												Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_THREADID),
												ComparisonType.EQUAL,
												this.id.ToString()
											),
											new ComparisonCondition(
												Post.TableSpec.instance.getIdSpec(),
												ComparisonType.GREATERTHAN,
												s
											),
											new ComparisonCondition(
												Post.TableSpec.instance.getIdSpec(),
												ComparisonType.LESSTHAN,
												minPost.id.ToString()
											)
										),
										new JoinSpec[0]
									);
									if(count > 0) {
										return (s == "0") ? null : s; //if there are some unread posts earlier than minPost
									} else {
										return maxPost.id.ToString();
									}
								}
							)
						}
					},
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							ReadMarkerTableSpec.instance.getColumnSpec(ReadMarkerTableSpec.FIELD_THREADID),
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
			});
		}

	}

}
