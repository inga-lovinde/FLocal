﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;
using FLocal.Common.actions;
using FLocal.Common.helpers;

namespace FLocal.Common.dataobjects {
	public class Thread : SqlObject<Thread> {

		public static readonly int FORMALREADMIN = Config.instance.MinPostId;

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

		private AbstractCondition getReadmarkerSearchCondition(Account account) {
			return new ComplexCondition(
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
			);
		}

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
		public string titleTranslit {
			get {
				return TranslitManager.Translit(this.title).PHPSubstring(0, 50);
			}
		}
		
		private int _lastPostId;
		public int lastPostId {
			get {
				this.LoadIfNotLoaded();
				return this._lastPostId;
			}
		}
		public Post lastPost {
			get {
				return Post.LoadById(this.lastPostId);
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
				return this._isLocked;
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
			this._isAnnouncement = Web.Core.Util.string2bool(data[TableSpec.FIELD_ISANNOUNCEMENT]);
			this._isLocked = Web.Core.Util.string2bool(data[TableSpec.FIELD_ISLOCKED]);
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._totalViews = int.Parse(data[TableSpec.FIELD_TOTALVIEWS]);
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("thread",
				new XElement("id", this.id),
				new XElement("name", this.title),
				new XElement("nameTranslit", this.titleTranslit),
				new XElement("parent", this.board.exportToXmlSimpleWithParent(context))
			);
		}

		public XElement exportToXml(UserContext context, params XElement[] additional) {

			if(context.isPostVisible(this.firstPost) != PostVisibilityEnum.VISIBLE) return null;

			XElement result = new XElement("thread",
				new XElement("id", this.id),
				new XElement("firstPost", this.firstPost.exportToXml(context)),
				new XElement("title", this.title),
				new XElement("lastPostId", this.lastPostId),
				new XElement("lastPostDate", this.lastPostDate.ToXml()),
				new XElement("isAnnouncement", this.isAnnouncement),
				new XElement("isLocked", this.isLocked),
				new XElement("totalPosts", this.totalPosts),
				new XElement("totalViews", this.totalViews),
				context.formatTotalPosts(this.totalPosts)
			);
			if(context.account != null) {
				int lastReadId = this.getLastReadId(context.account);
				result.Add(
					new XElement("afterLastRead", lastReadId + 1),
					new XElement(
						"totalNewPosts",
						Config.instance.mainConnection.GetCountByConditions(
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
									lastReadId.ToString()
								)
							)
						)
					)
				);
			}
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		public IEnumerable<Post> getPosts(Diapasone diapasone, bool isAscending) {
			diapasone.total = this.totalPosts;
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
							isAscending
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

		private readonly Dictionary<int, int> lastReadId_cache = new Dictionary<int, int>();
		private int do_getLastReadId(Account account) {
			List<string> stringIds = Config.instance.mainConnection.LoadIdsByConditions(
				ReadMarkerTableSpec.instance,
				this.getReadmarkerSearchCondition(account),
				Diapasone.unlimited
			);
			if(stringIds.Count > 1) {
				throw new CriticalException("more than one row");
			}
			if(stringIds.Count < 1) {
				return (this.lastPostId >= FORMALREADMIN) ? 0 : FORMALREADMIN;
			}
			Dictionary<string, string> data = Config.instance.mainConnection.LoadById(ReadMarkerTableSpec.instance, stringIds[0]);
			if((data[ReadMarkerTableSpec.FIELD_POSTID] == "") || (data[ReadMarkerTableSpec.FIELD_POSTID] == null)) {
				return 0;
			}
			return int.Parse(data[ReadMarkerTableSpec.FIELD_POSTID]);
		}
		public int getLastReadId(Account account) {
			if(!this.lastReadId_cache.ContainsKey(account.id)) {
				lock(this.lastReadId_cache) {
					if(!this.lastReadId_cache.ContainsKey(account.id)) {
						this.lastReadId_cache[account.id] = this.do_getLastReadId(account);
					}
				}
			}
			return this.lastReadId_cache[account.id];
		}
		private void lastReadId_Reset(Account account, int _lastReadId) {
			int lastReadId = _lastReadId;
			lock(this.lastReadId_cache) {
				if(this.lastReadId_cache.ContainsKey(account.id)) {
					lastReadId = Math.Max(lastReadId, _lastReadId);
				}
				this.lastReadId_cache[account.id] = lastReadId;
			}
		}

		public void forceMarkAsRead(Account account, Post maxPost) {
			var actualLastRead = new Box<string>();
			ChangeSetUtil.ApplyChanges(
				new InsertOrUpdateChange(
					ReadMarkerTableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ ReadMarkerTableSpec.FIELD_ACCOUNTID, new ScalarFieldValue(account.id.ToString()) },
						{ ReadMarkerTableSpec.FIELD_THREADID, new ScalarFieldValue(this.id.ToString()) },
						{ ReadMarkerTableSpec.FIELD_POSTID, new ScalarFieldValue(maxPost.id.ToString()) },
					},
					new Dictionary<string,AbstractFieldValue> {
						{ ReadMarkerTableSpec.FIELD_POSTID, new IncrementFieldValue(IncrementFieldValue.GREATEST(maxPost.id), actualLastRead) },
					},
					this.getReadmarkerSearchCondition(account)
				)
			);
			int actualLastReadId = 0;
			int.TryParse(actualLastRead.value, out actualLastReadId);
			this.lastReadId_Reset(account, actualLastReadId);
		}

		public void markAsRead(Account account, Post minPost, Post maxPost) {
			Box<string> actualLastRead = new Box<string>();
			ChangeSetUtil.ApplyChanges(
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
										)
									);
									if(count > 0) {
										return (s == "0") ? null : s; //if there are some unread posts earlier than minPost
									} else {
										return maxPost.id.ToString();
									}
								},
								actualLastRead
							)
						}
					},
					this.getReadmarkerSearchCondition(account)
				)
			);
			int actualLastReadId = 0;
			int.TryParse(actualLastRead.value, out actualLastReadId);
			this.lastReadId_Reset(account, actualLastReadId);
		}

		internal static KeyValuePair<AbstractChange, IEnumerable<AbstractChange>> getNewPostChanges(Board board, int threadId, Post parentPost, User poster, PostLayer layer, string title, string body, DateTime date, int? forcedPostId) {
			string parentPostId = null;
			if(parentPost != null) parentPostId = parentPost.id.ToString();
			bool isNewThread = (parentPost == null);
			HashSet<int> mentionedUsersIds = new HashSet<int>();
			if(parentPost != null && parentPost.poster.id != poster.id) {
				mentionedUsersIds.Add(parentPost.poster.id);
			}
			string bodyIntermediate;
			if(forcedPostId.HasValue) {
				//dirty hack
				bodyIntermediate = body;
			} else {
				bodyIntermediate = UBBParser.UBBToIntermediate(
					new DelegatePostParsingContext(mentionedUser => mentionedUsersIds.Add(mentionedUser.id)),
					body
				);
			}
			var postInsertData = new Dictionary<string,AbstractFieldValue> {
				{ Post.TableSpec.FIELD_THREADID, new ScalarFieldValue(threadId.ToString()) },
				{ Post.TableSpec.FIELD_PARENTPOSTID, new ScalarFieldValue(parentPostId) },
				{ Post.TableSpec.FIELD_POSTERID, new ScalarFieldValue(poster.id.ToString()) },
				{ Post.TableSpec.FIELD_POSTDATE, new ScalarFieldValue(date.ToUTCString()) },
				//{ Post.TableSpec.FIELD_REVISION, new ScalarFieldValue("0") },
				{ Post.TableSpec.FIELD_LASTCHANGEDATE, new ScalarFieldValue(date.ToUTCString()) },
				{ Post.TableSpec.FIELD_LAYERID, new ScalarFieldValue(layer.id.ToString()) },
				{ Post.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
				{ Post.TableSpec.FIELD_BODY, new ScalarFieldValue(bodyIntermediate) },
				{ Post.TableSpec.FIELD_TOTALPUNISHMENTS, new ScalarFieldValue("0") },
			};
			if(!forcedPostId.HasValue) {
				postInsertData[Post.TableSpec.FIELD_REVISION] = new ScalarFieldValue("0");
			} else {
				postInsertData[Post.TableSpec.FIELD_ID] = new ScalarFieldValue(forcedPostId.Value.ToString());
			}
			AbstractChange postInsert = new InsertChange(
				Post.TableSpec.instance,
				postInsertData
			);
			AbstractFieldValue postReference = new ReferenceFieldValue(postInsert);
			AbstractFieldValue postIndexReference = new TwoWayReferenceFieldValue(postInsert, TwoWayReferenceFieldValue.GREATEST);
			AbstractChange revisionInsert = new InsertChange(
				Revision.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ Revision.TableSpec.FIELD_POSTID, postReference },
					{ Revision.TableSpec.FIELD_NUMBER, new ScalarFieldValue("0") },
					{ Revision.TableSpec.FIELD_CHANGEDATE, new ScalarFieldValue(date.ToUTCString()) },
					{ Revision.TableSpec.FIELD_TITLE, new ScalarFieldValue(title) },
					{ Revision.TableSpec.FIELD_BODY, new ScalarFieldValue(body) },
				}
			);
			Dictionary<string, AbstractFieldValue> threadData = new Dictionary<string,AbstractFieldValue> {
				{ Thread.TableSpec.FIELD_LASTPOSTDATE, new ScalarFieldValue(date.ToUTCString()) },
				{ Thread.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue() },
				{
					Thread.TableSpec.FIELD_LASTPOSTID,
					postIndexReference
				}
			};
			if(isNewThread) {
				threadData[Thread.TableSpec.FIELD_FIRSTPOSTID] = postReference;
			}
			AbstractChange threadUpdate = new UpdateChange(
				Thread.TableSpec.instance,
				threadData,
				threadId
			);
			AbstractChange userUpdate = new UpdateChange(
				User.TableSpec.instance,
				new Dictionary<string,AbstractFieldValue> {
					{ User.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue() },
				},
				poster.id
			);

			List<AbstractChange> changes = new List<AbstractChange>();
			changes.Add(postInsert);
			if(!forcedPostId.HasValue) {
				//dirty hack
				changes.Add(revisionInsert);
			}
			changes.Add(threadUpdate);
			changes.Add(userUpdate);

			foreach(var mentionedUserId in mentionedUsersIds) {
				changes.Add(
					new InsertChange(
						Mention.TableSpec.instance,
						new Dictionary<string, AbstractFieldValue> {
							{ Mention.TableSpec.FIELD_MENTIONEDUSERID, new ScalarFieldValue(mentionedUserId.ToString()) },
							{ Mention.TableSpec.FIELD_POSTID, new ReferenceFieldValue(postInsert) },
							{ Mention.TableSpec.FIELD_DATE, new ScalarFieldValue(date.ToUTCString()) },
						}
					)
				);
			}

			Dictionary<string, AbstractFieldValue> boardData = new Dictionary<string,AbstractFieldValue> {
				{ Board.TableSpec.FIELD_TOTALPOSTS, new IncrementFieldValue() },
				{
					Board.TableSpec.FIELD_LASTPOSTID,
					postIndexReference
				}
			};
			if(isNewThread) {
				boardData[Board.TableSpec.FIELD_TOTALTHREADS] = new IncrementFieldValue();
			}
			int? boardId = board.id;
			do {
				Board _board = Board.LoadById(boardId.Value);
				changes.Add(
					new UpdateChange(
						Board.TableSpec.instance,
						boardData,
						_board.id
					)
				);
				boardId = _board.parentBoardId;
			} while(boardId.HasValue);

			return new KeyValuePair<AbstractChange,IEnumerable<AbstractChange>>(postInsert, changes);
		}

		public readonly object locker = new object();

		public void Synchronized(Action action) {
			lock(this.locker) {
				this.board.Synchronized(action);
			}
		}

	}

}
