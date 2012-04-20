using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Web.Core;
using Web.Core.DB;
using Web.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class Mention : SqlObject<Mention> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Mentions";
			public const string FIELD_ID = "Id";
			public const string FIELD_MENTIONEDUSERID = "MentionedUserId";
			public const string FIELD_POSTID = "PostId";
			public const string FIELD_DATE = "Date";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _mentionedUserId;
		public int mentionedUserId {
			get {
				this.LoadIfNotLoaded();
				return this._mentionedUserId;
			}
		}
		public User mentionedUser {
			get {
				return User.LoadById(this.mentionedUserId);
			}
		}

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

		private DateTime _date;
		public DateTime date {
			get {
				this.LoadIfNotLoaded();
				return this._date;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._mentionedUserId = int.Parse(data[TableSpec.FIELD_MENTIONEDUSERID]);
			this._postId = int.Parse(data[TableSpec.FIELD_POSTID]);
			this._date = Util.ParseDateTimeFromTimestamp(data[TableSpec.FIELD_DATE]).Value;
		}

	}
}
