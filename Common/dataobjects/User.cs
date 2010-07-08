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

		public const string ENUM_SHOWPOSTSTOUSERS_ALL = "All";
		public const string ENUM_SHOWPOSTSTOUSERS_LOGGEDIN = "LoggedIn";
		public const string ENUM_SHOWPOSTSTOUSERS_PRIVELEGED = "Priveleged";
		public const string ENUM_SHOWPOSTSTOUSERS_NONE = "None";

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
			public const string FIELD_BIOGRAPHY = "Biography";
			public const string FIELD_AVATARID = "AvatarId";
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

		private string _biography;
		public string biography {
			get {
				this.LoadIfNotLoaded();
				return this._biography;
			}
		}

		private int? _avatarId;
		public int? avatarId {
			get {
				this.LoadIfNotLoaded();
				return this._avatarId;
			}
		}
		public Upload avatar {
			get {
				return Upload.LoadById(this.avatarId.Value);
			}
		}

		private static Dictionary<string, int> username2id = new Dictionary<string,int>();
		public static User LoadByName(string name) {
			if(!username2id.ContainsKey(name)) {
				lock(username2id) {
					if(!username2id.ContainsKey(name)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_NAME),
								ComparisonType.EQUAL,
								name
							),
							Diapasone.unlimited
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							username2id[name] = int.Parse(ids[0]);
						} else {
							throw new NotFoundInDBException();
						}
					}
				}
			}
			return User.LoadById(username2id[name]);
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
			this._biography = data[TableSpec.FIELD_BIOGRAPHY];
			this._avatarId = Util.ParseInt(data[TableSpec.FIELD_AVATARID]);
		}

		public XElement exportToXmlForViewing(UserContext context, params XElement[] additional) {
			XElement result = new XElement("user",
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
			if(this.avatarId.HasValue) {
				result.Add(new XElement("avatar", this.avatarId));
			}
			foreach(XElement elem in additional) {
				result.Add(elem);
			}
			return result;
		}

		public static IEnumerable<User> getUsers(Diapasone diapasone) {
			return User.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					User.TableSpec.instance,
					new EmptyCondition(),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							User.TableSpec.instance.getIdSpec(),
							true
						),
					}
				) select int.Parse(stringId)
			);
		}

		public PostLayer getActualLayer(Board board, PostLayer desiredLayer) {
			return desiredLayer;
		}

		public IEnumerable<Post> getPosts(Diapasone diapasone) {
			return Post.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Post.TableSpec.instance,
					new ComparisonCondition(
						Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_POSTERID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Post.TableSpec.instance.getIdSpec(),
							false
						),
					}
				) select int.Parse(stringId)
			);
		}

		public IEnumerable<Post> getReplies(Diapasone diapasone) {
			JoinSpec parent = new JoinSpec(
				Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_PARENTPOSTID),
				Post.TableSpec.instance,
				"parent"
			);
			return Post.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Post.TableSpec.instance,
					new ComparisonCondition(
						parent.additionalTable.getColumnSpec(Post.TableSpec.FIELD_POSTERID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					diapasone,
					new JoinSpec[] {
						parent
					},
					new SortSpec[] {
						new SortSpec(
							Post.TableSpec.instance.getIdSpec(),
							false
						),
					}
				) select int.Parse(stringId)
			);
		}

	}
}
