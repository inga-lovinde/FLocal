using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class User : SqlObject<User> {

		public class TableSpec : ISqlObjectTableSpec {
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
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

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

		private static Dictionary<string, int> id2user = new Dictionary<string,int>();
		public static User LoadByName(string name) {
			if(!id2user.ContainsKey(name)) {
				lock(id2user) {
					if(!id2user.ContainsKey(name)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_NAME),
								ComparisonType.EQUAL,
								name
							),
							Diapasone.unlimited,
							new JoinSpec[0]
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							id2user[name] = int.Parse(ids[0]);
						} else {
							throw new NotFoundInDBException();
						}
					}
				}
			}
			return User.LoadById(id2user[name]);
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
