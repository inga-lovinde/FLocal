using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common.dataobjects {
	public class Post : SqlObject<Post> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Posts";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTERID = "PosterId";
			public const string FIELD_POSTDATE = "PostDate";
			public const string FIELD_LASTCHANGEDATE = "LastChangeDate";
			public const string FIELD_REVISION = "Revision";
			public const string FIELD_LAYER = "Layer";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_THREADID = "ThreadId";
			public const string FIELD_PARENTPOSTID = "ParentPostId";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
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
		
		private int _layer;
		public int layer {
			get {
				this.LoadIfNotLoaded();
				return this._layer;
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
				return this.body.Replace("<br />", Util.EOL).Substring(0, Math.Min(1000, this.body.IndexOf("<")));
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
			this._layer = int.Parse(data[TableSpec.FIELD_LAYER]);
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

		public XElement exportToXmlWithoutThread(UserContext context, bool includeParentPost) {
			XElement result = new XElement("post",
				new XElement("id", this.id),
				new XElement("poster", this.poster.exportToXmlForViewing(context)),
				new XElement("postDate", this.postDate.ToXml()),
				new XElement("lastChangeDate", this.postDate.ToXml()),
				new XElement("revision", this.revision),
				new XElement("layer", this.layer),
				new XElement("title", this.title),
				new XElement("body", this.body),
				new XElement("bodyShort", this.bodyShort),
				new XElement("threadId", this.threadId)
			);
			if(includeParentPost) {
				if(this.parentPostId.HasValue) {
					result.Add(new XElement("parentPost", this.parentPost.exportToXmlWithoutThread(context, false)));
				}
			}
			return result;
		}

	}
}
