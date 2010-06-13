using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.Common.dataobjects {
	public class User : SqlObject<User> {

		public class TableSpec : FLocal.Core.DB.ITableSpec {
			public const string TABLE = "Users";
			public const string FIELD_ID = "Id";
			public const string FIELD_REGDATE = "RegDate";
			public const string FIELD_TOTALPOSTS = "TotalPosts";
			public const string FIELD_SIGNATURE = "Signature";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_LOCATION = "Location";
			public const string FIELD_NAME = "Name";
			public const string FIELD_USERGROUPID = "UserGroupId";
			public const string FIELD_SHOWPOSTSTOUSERS = "ShowPostsToUsers";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
		}

		protected override FLocal.Core.DB.ITableSpec table { get { return TableSpec.instance; } }

		private DateTime _regDate;
		public DateTime regDate {
			get {
				this.LoadIfNotLoaded();
				return this._regDate;
			}
		}

		private int _totalPosts;
		public int totalPosts {
			get {
				this.LoadIfNotLoaded();
				return this._totalPosts;
			}
		}

		private string _signature;
		public string signature {
			get {
				this.LoadIfNotLoaded();
				return this._signature;
			}
		}

		private string _title;
		public string title {
			get {
				this.LoadIfNotLoaded();
				return this._title;
			}
		}

		private string _location;
		public string location {
			get {
				this.LoadIfNotLoaded();
				return this._location;
			}
		}

		private string _name;
		public string name {
			get {
				this.LoadIfNotLoaded();
				return this._name;
			}
		}

		private int _userGroupId;
		public int userGroupId {
			get {
				this.LoadIfNotLoaded();
				return this._userGroupId;
			}
		}

		private string _showPostsToUsers;
		public string showPostsToUsers {
			get {
				this.LoadIfNotLoaded();
				return this._showPostsToUsers;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._regDate = new DateTime(long.Parse(data[TableSpec.FIELD_REGDATE]));
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._signature = data[TableSpec.FIELD_SIGNATURE];
			this._title = data[TableSpec.FIELD_TITLE];
			this._location = data[TableSpec.FIELD_LOCATION];
			this._name = data[TableSpec.FIELD_NAME];
			this._userGroupId = int.Parse(data[TableSpec.FIELD_USERGROUPID]);
			this._showPostsToUsers = data[TableSpec.FIELD_SHOWPOSTSTOUSERS];
		}

		public XElement exportToXmlForViewing(UserContext context) {
			return new XElement("user",
				new XElement("id", this.id),
				new XElement("regDate", this.regDate.ToXml()),
				new XElement("totalPosts", this.totalPosts),
				new XElement("signature", this.signature),
				new XElement("title", this.title),
				new XElement("location", this.location),
				new XElement("name", this.name),
				new XElement("userGroupId", this.userGroupId),
				new XElement("showPostsToUsers", this.showPostsToUsers)
			);
		}

	}
}
