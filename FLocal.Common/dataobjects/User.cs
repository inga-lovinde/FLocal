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
	public class User : SqlObject<User> {

		public const string ENUM_SHOWPOSTSTOUSERS_ALL = "All";
		public const string ENUM_SHOWPOSTSTOUSERS_LOGGEDIN = "LoggedIn";
		public const string ENUM_SHOWPOSTSTOUSERS_PRIVELEGED = "Priveleged";
		public const string ENUM_SHOWPOSTSTOUSERS_NONE = "None";

		public struct UserData {
			public string title;
			public string location;
			public string signatureUbb;
			public string biographyUbb;
		}

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Users";
			public const string FIELD_ID = "Id";
			public const string FIELD_REGDATE = "RegDate";
			public const string FIELD_TOTALPOSTS = "TotalPosts";
			public const string FIELD_SIGNATURE = "Signature";
			public const string FIELD_SIGNATUREUBB = "SignatureUbb";
			public const string FIELD_TITLE = "Title";
			public const string FIELD_LOCATION = "Location";
			public const string FIELD_NAME = "Name";
			public const string FIELD_USERGROUPID = "UserGroupId";
			public const string FIELD_SHOWPOSTSTOUSERS = "ShowPostsToUsers";
			public const string FIELD_BIOGRAPHY = "Biography";
			public const string FIELD_BIOGRAPHYUBB = "BiographyUbb";
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

		private string _signatureUbb;
		public string signatureUbb {
			get {
				this.LoadIfNotLoaded();
				return this._signatureUbb;
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
		public UserGroup userGroup {
			get {
				return UserGroup.LoadById(this.userGroupId);
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

		private string _biographyUbb;
		public string biographyUbb {
			get {
				this.LoadIfNotLoaded();
				return this._biographyUbb;
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
						username2id[name] = int.Parse(
							Config.instance.mainConnection.LoadIdByField(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_NAME),
								name
							)
						);
					}
				}
			}
			return User.LoadById(username2id[name]);
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._regDate = new DateTime(long.Parse(data[TableSpec.FIELD_REGDATE]));
			this._totalPosts = int.Parse(data[TableSpec.FIELD_TOTALPOSTS]);
			this._signature = data[TableSpec.FIELD_SIGNATURE];
			this._signatureUbb = data[TableSpec.FIELD_SIGNATUREUBB];
			this._title = data[TableSpec.FIELD_TITLE];
			this._location = data[TableSpec.FIELD_LOCATION];
			this._name = data[TableSpec.FIELD_NAME];
			this._userGroupId = int.Parse(data[TableSpec.FIELD_USERGROUPID]);
			this._showPostsToUsers = data[TableSpec.FIELD_SHOWPOSTSTOUSERS];
			this._biography = data[TableSpec.FIELD_BIOGRAPHY];
			this._biographyUbb = data[TableSpec.FIELD_BIOGRAPHYUBB];
			this._avatarId = Util.ParseInt(data[TableSpec.FIELD_AVATARID]);
		}

		public XElement exportToXmlForViewing(UserContext context, params XElement[] additional) {
			XElement result = new XElement("user",
				new XElement("id", this.id),
				new XElement("regDate", this.regDate.ToXml()),
				new XElement("totalPosts", this.totalPosts),
				new XElement("signature", context.outputParams.preprocessBodyIntermediate(this.signature)),
				new XElement("signatureUbb", this.signatureUbb),
				new XElement("biography", context.outputParams.preprocessBodyIntermediate(this.biography)),
				new XElement("biographyUbb", this.biographyUbb),
				new XElement("title", this.title),
				new XElement("location", this.location),
				new XElement("name", this.name),
				this.userGroup.exportToXml(context),
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

		public static IEnumerable<User> getUsers(Diapasone diapasone, bool isAscending) {
			return User.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					User.TableSpec.instance,
					new EmptyCondition(),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							User.TableSpec.instance.getIdSpec(),
							isAscending
						),
					}
				) select int.Parse(stringId)
			);
		}

		public PostLayer getActualLayer(Board board, PostLayer desiredLayer) {
			Dictionary<int, DateTime> restrictionData = Restriction.GetRestrictionData(this, board);
			if(restrictionData.ContainsKey(desiredLayer.id) && (restrictionData[desiredLayer.id].CompareTo(DateTime.Now) >= 0)) throw new FLocalException("You're restricted from posting in this layer until " + restrictionData[desiredLayer.id].ToString());
			return desiredLayer;
		}

		public IEnumerable<Post> getPosts(Diapasone diapasone, bool isAscending) {
			diapasone.total = this.totalPosts;
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
							isAscending
						),
					}
				) select int.Parse(stringId)
			);
		}

		public IEnumerable<Post> getReplies(Diapasone diapasone, bool isAscending) {
			JoinSpec parent = new JoinSpec(
				Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_PARENTPOSTID),
				Post.TableSpec.instance,
				"parent"
			);
			return Post.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Post.TableSpec.instance,
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							parent.additionalTable.getColumnSpec(Post.TableSpec.FIELD_POSTERID),
							ComparisonType.EQUAL,
							this.id.ToString()
						),
						new ComparisonCondition(
							Post.TableSpec.instance.getColumnSpec(Post.TableSpec.FIELD_POSTERID),
							ComparisonType.NOTEQUAL,
							this.id.ToString()
						)
					),
					diapasone,
					new JoinSpec[] {
						parent
					},
					new SortSpec[] {
						new SortSpec(
							Post.TableSpec.instance.getIdSpec(),
							isAscending
						),
					}
				) select int.Parse(stringId)
			);
		}

		public IEnumerable<Thread> getThreads(Diapasone diapasone, bool isAscending) {
			return Thread.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Thread.TableSpec.instance,
					new ComparisonCondition(
						Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_TOPICSTARTERID),
						ComparisonType.EQUAL,
						this.id.ToString()
					),
					diapasone,
					new JoinSpec[0],
					new SortSpec[] {
						new SortSpec(
							Thread.TableSpec.instance.getColumnSpec(Thread.TableSpec.FIELD_LASTPOSTID),
							isAscending
						),
					}
				) select int.Parse(stringId)
			);
		}

		public void UpdateData(UserData newData) {
			if(newData.location.Length > 30) throw new FLocalException("Location is too long");
			if(newData.title.Length > 30) throw new FLocalException("Title is too long");
			if(newData.signatureUbb.Length > 1024) throw new FLocalException("Signature is too long");
			ChangeSetUtil.ApplyChanges(
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ TableSpec.FIELD_LOCATION, new ScalarFieldValue(newData.location) },
						{ TableSpec.FIELD_TITLE, new ScalarFieldValue(newData.title) },
						{ TableSpec.FIELD_BIOGRAPHYUBB, new ScalarFieldValue(newData.biographyUbb) },
						{ TableSpec.FIELD_BIOGRAPHY, new ScalarFieldValue(UBBParser.UBBToIntermediate(newData.biographyUbb)) },
						{ TableSpec.FIELD_SIGNATUREUBB, new ScalarFieldValue(newData.signatureUbb) },
						{ TableSpec.FIELD_SIGNATURE, new ScalarFieldValue(UBBParser.UBBToIntermediate(newData.signatureUbb)) },
					},
					this.id
				)
			);
		}

		public void SetAvatar(Upload avatar) {

			if((avatar != null) && (avatar.size > Upload.AVATAR_MAX_FILESIZE)) throw new FLocalException("Avatar is too big (max. 80KB allowed)");

			ChangeSetUtil.ApplyChanges(
				new UpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ TableSpec.FIELD_AVATARID, new ScalarFieldValue((avatar != null) ? avatar.id.ToString() : null) },
					},
					this.id
				)
			);
		}

		public IEnumerable<Punishment> getPunishments(Diapasone diapasone) {
			return Punishment.LoadByIds(
				from stringId in Config.instance.mainConnection.LoadIdsByConditions(
					Punishment.TableSpec.instance,
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							Punishment.TableSpec.instance.getColumnSpec(Punishment.TableSpec.FIELD_OWNERID),
							ComparisonType.EQUAL,
							this.id.ToString()
						),
						new ComparisonCondition(
							Punishment.TableSpec.instance.getColumnSpec(Punishment.TableSpec.FIELD_EXPIRES),
							ComparisonType.GREATEROREQUAL,
							DateTime.Now.ToUTCString()
						),
						new ComparisonCondition(
							Punishment.TableSpec.instance.getColumnSpec(Punishment.TableSpec.FIELD_ISWITHDRAWED),
							ComparisonType.EQUAL,
							"0"
						)
					),
					diapasone
				) select int.Parse(stringId)
			);
		}

	}
}
