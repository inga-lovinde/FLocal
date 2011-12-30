using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using System.Xml.Linq;

namespace FLocal.Common.dataobjects {
	public class Revision : SqlObject<Revision> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Revisions";
			public const string FIELD_ID = "Id";
			public const string FIELD_POSTID = "PostId";
			public const string FIELD_CHANGEDATE = "ChangeDate";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_BODY = "Body";
			public const string FIELD_NUMBER = "Number";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _postId;
		public int postId {
			get {
				this.LoadIfNotLoaded();
				return this._postId;
			}
		}
		public Post post {
			get {
				return Post.LoadById(this.postId);
			}
		}

		private DateTime _changeDate;
		public DateTime changeDate {
			get {
				this.LoadIfNotLoaded();
				return this._changeDate;
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

		private int _number;
		public int number {
			get {
				this.LoadIfNotLoaded();
				return this._number;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._postId = int.Parse(data[TableSpec.FIELD_POSTID]);
			this._changeDate = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_CHANGEDATE]).Value;
			this._title = data[TableSpec.FIELD_TITLE];
			this._body = data[TableSpec.FIELD_BODY];
			this._number = int.Parse(data[TableSpec.FIELD_NUMBER]);
		}


		public XElement exportToXml(UserContext context) {
			return new XElement("revision",
				new XElement("id", this.id),
				new XElement("title", this.title),
				new XElement("body", this.body)
			);
		}
	}
}
