using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Common;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class Post : SqlObject<Post> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Posts";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTERID = "PosterId";
			public const string FIELD_POSTDATE = "PostDate";
			public const string FIELD_LASTCHANGEDATE = "LastChangeDate";
			public const string FIELD_REVISION = "Revision";
			public const string FIELD_LAYERID = "LayerId";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_THREADID = "ThreadId";
			public const string FIELD_PARENTPOSTID = "ParentPostId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		public class RevisionTableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Revisions";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTID = "PostId";
			public const string FIELD_CHANGEDATE = "ChangeDate";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_NUMBER = "Number";
			public static readonly RevisionTableSpec instance = new RevisionTableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _posterId;
		public int posterId {
			get {
				this.LoadIfNotLoaded();
				return this._posterId;
			}
		}
		public User poster {
			get {
				this.LoadIfNotLoaded();
				return User.LoadById(this.posterId);
			}
		}

		private DateTime _postDate;
		public DateTime postDate {
			get {
				this.LoadIfNotLoaded();
				return this._postDate;
			}
		}

		private DateTime? _lastChangeDate;
		public DateTime? lastChangeDate {
			get {
				this.LoadIfNotLoaded();
				return this._lastChangeDate;
			}
		}

		private int _revision;
		public int revision {
			get {
				this.LoadIfNotLoaded();
				return this._revision;
			}
		}
		
		private int _layerId;
		public int layerId {
			get {
				this.LoadIfNotLoaded();
				return this._layerId;
			}
		}
		public PostLayer layer {
			get {
				return PostLayer.LoadById(this.layerId);
			}
		}

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}

		private string _body;
		public string body {
			get {
				this.LoadIfNotLoaded();
				return this._body;
			}
		}
		public string bodyShort {
			get {
				return this.body.Replace("<br />", Util.EOL).PHPSubstring(0, 1000);
			}
		}

		private int _threadId;
		public int threadId {
			get {
				this.LoadIfNotLoaded();
				return this._threadId;
			}
		}
		public Thread thread {
			get {
				return Thread.LoadById(this.threadId);
			}
		}

		private int? _parentPostId;
		public int? parentPostId {
			get {
				this.LoadIfNotLoaded();
				return this._parentPostId;
			}
		}
		public Post parentPost {
			get {
				return Post.LoadById(this.parentPostId.Value);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._posterId = int.Parse(data[TableSpec.FIELD_POSTERID]);
			this._postDate = new DateTime(long.Parse(data[TableSpec.FIELD_POSTDATE]));
			this._lastChangeDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_LASTCHANGEDATE]);
			this._revision = int.Parse(data[TableSpec.FIELD_REVISION]);
			this._layerId = int.Parse(data[TableSpec.FIELD_LAYERID]);
			this._title = data[TableSpec.FIELD_TITLE];
			this._body = data[TableSpec.FIELD_BODY];
			this._threadId = int.Parse(data[TableSpec.FIELD_THREADID]);
			this._parentPostId = Util.ParseInt(data[TableSpec.FIELD_PARENTPOSTID]);
		}

		public XElement exportToXmlSimpleWithParent(UserContext context) {
			return new XElement("post",
				new XElement("id", this.id),
				new XElement("name", this.title),
				new XElement("parent", this.thread.exportToXmlSimpleWithParent(context))
			);
		}

		private XNode XMLBody(UserContext context) {
			return XElement.Parse("<body>" + context.outputParams.preprocessBodyIntermediate(this.body) + "</body>", LoadOptions.PreserveWhitespace);
		}

		public XElement exportToXmlWithoutThread(UserContext context, bool includeParentPost, params XElement[] additional) {
			
			if(!context.isPostVisible(this)) return null;

			XElement result = new XElement("post",
				new XElement("id", this.id),
				new XElement("poster", this.poster.exportToXmlForViewing(context)),
				new XElement("postDate", this.postDate.ToXml()),
				new XElement("lastChangeDate", this.postDate.ToXml()),
				new XElement("revision", this.revision),
				new XElement("layerId", this.layerId),
				new XElement("layerName", this.layer.name),
				new XElement("title", this.title),
				new XElement("body", context.outputParams.preprocessBodyIntermediate(this.body)),
				//this.XMLBody(context),
				new XElement("bodyShort", this.bodyShort),
				new XElement("threadId", this.threadId)
			);
			if(includeParentPost) {
				if(this.parentPostId.HasValue) {
					result.Add(new XElement("parentPost", this.parentPost.exportToXmlWithoutThread(context, false)));
				}
			}
			if(additional.Length > 0) {
				result.Add(additional);
			}
			return result;
		}

		public Post Reply(User poster, string title, string body, PostLayer desiredLayer) {
			return this.Reply(poster, title, body, desiredLayer, DateTime.Now, null);
		}
		public Post Reply(User poster, string title, string body, PostLayer desiredLayer, DateTime date, int? forcedPostId) {

			if(this.thread.isLocked) {
				throw new FLocalException("thread locked");
			}

			PostLayer actualLayer = poster.getActualLayer(this.thread.board, desiredLayer);

			var changes = Thread.getNewPostChanges(this.thread.board, this.threadId, this, poster, actualLayer, title, body, date, forcedPostId);
			ChangeSetUtil.ApplyChanges(changes.Value.ToArray());
			
			return Post.LoadById(changes.Key.getId().Value);
		}

	}
}
