using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class Thread : SqlObject<Thread> {

		public class TableSpec : FLocal.Core.DB.ITableSpec {
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
		}

		protected override FLocal.Core.DB.ITableSpec table { get { return TableSpec.instance; } }

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

		private bool hasNewPosts() {
			return Core.Util.RandomInt(0, 1000) < 500;
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("thread",
				new XElement("id", this.id),
				new XElement("name", this.title),
				new XElement("parent", this.board.exportToXmlSimpleWithParent(context))
			);
		}

		public XElement exportToXml(UserContext context) {
			return new XElement("thread",
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
				new XElement("hasNewPosts", this.hasNewPosts()),
				new XElement("bodyShort", this.firstPost.bodyShort),
				context.formatTotalPosts(this.totalPosts)
			);
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

	}
}
