using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;
using FLocal.Common.actions;

namespace FLocal.Common.dataobjects {
	public class AccountSettings : SqlObject<AccountSettings>, IUserSettings {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Accounts_Settings";
			public const string FIELD_ID = "Id";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_POSTSPERPAGE = "PostsPerPage";
			public const string FIELD_THREADSPERPAGE = "ThreadsPerPage";
			public const string FIELD_UPLOADSPERPAGE = "UploadsPerPage";
			public const string FIELD_USERSPERPAGE = "UsersPerPage";
			public const string FIELD_BOARDSVIEWSETTINGS = "BoardsViewSettings";
			public const string FIELD_SKINID = "SkinId";
			public const string FIELD_MODERNSKINID = "ModernSkinId";
			public const string FIELD_MACHICHARAID = "MachicharaId";
			public const string FIELD_MAXUPLOADIMAGEWIDTH = "MaxUploadImageWidth";
			public const string FIELD_MAXUPLOADIMAGEHEIGHT = "MaxUploadImageHeight";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) { Refresh(id); }
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _accountId;
		public int accountId {
			get {
				this.LoadIfNotLoaded();
				return this._accountId;
			}
		}
		public Account account {
			get {
				return Account.LoadById(this._accountId);
			}
		}
		
		private int _postsPerPage;
		public int postsPerPage {
			get {
				this.LoadIfNotLoaded();
				return this._postsPerPage;
			}
		}

		private int _threadsPerPage;
		public int threadsPerPage {
			get {
				this.LoadIfNotLoaded();
				return this._threadsPerPage;
			}
		}

		private int _uploadsPerPage;
		public int uploadsPerPage {
			get {
				this.LoadIfNotLoaded();
				return this._uploadsPerPage;
			}
		}

		private int _usersPerPage;
		public int usersPerPage {
			get {
				this.LoadIfNotLoaded();
				return this._usersPerPage;
			}
		}

		private string _boardsViewSettings;

		private int _skinId;
		public int skinId {
			get {
				this.LoadIfNotLoaded();
				return this._skinId;
			}
		}
		public Skin skin {
			get {
				return Skin.LoadById(this.skinId);
			}
		}

		private int _modernSkinId;
		public int modernSkinId {
			get {
				this.LoadIfNotLoaded();
				return this._modernSkinId;
			}
		}
		public ModernSkin modernSkin {
			get {
				return ModernSkin.LoadById(this.modernSkinId);
			}
		}

		private int _machicharaId;
		public int machicharaId {
			get {
				this.LoadIfNotLoaded();
				return this._machicharaId;
			}
		}
		public Machichara machichara {
			get {
				return Machichara.LoadById(this.machicharaId);
			}
		}

		private int _maxUploadImageWidth;
		public int maxUploadImageWidth {
			get {
				this.LoadIfNotLoaded();
				return this._maxUploadImageWidth;
			}
		}

		private int _maxUploadImageHeight;
		public int maxUploadImageHeight {
			get {
				this.LoadIfNotLoaded();
				return this._maxUploadImageHeight;
			}
		}

		public bool isPostVisible(Post post) {
			if(post.layer.name == PostLayer.NAME_HIDDEN) return false;
			if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_NONE) return false;
			if(post.poster.showPostsToUsers == User.ENUM_SHOWPOSTSTOUSERS_PRIVELEGED) return this.account.user.userGroup.name == UserGroup.NAME_JUDGES || this.account.user.userGroup.name == UserGroup.NAME_ADMINISTRATORS;
			return true;
		}
		
		protected override void doFromHash(Dictionary<string, string> data) {
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
			this._postsPerPage = int.Parse(data[TableSpec.FIELD_POSTSPERPAGE]);
			this._threadsPerPage = int.Parse(data[TableSpec.FIELD_THREADSPERPAGE]);
			this._uploadsPerPage = int.Parse(data[TableSpec.FIELD_UPLOADSPERPAGE]);
			this._usersPerPage = int.Parse(data[TableSpec.FIELD_USERSPERPAGE]);
			this._boardsViewSettings = data[TableSpec.FIELD_BOARDSVIEWSETTINGS];
			this._skinId = int.Parse(data[TableSpec.FIELD_SKINID]);
			this._modernSkinId = int.Parse(data[TableSpec.FIELD_MODERNSKINID]);
			this._machicharaId = int.Parse(data[TableSpec.FIELD_MACHICHARAID]);
			this._maxUploadImageWidth = int.Parse(data[TableSpec.FIELD_MAXUPLOADIMAGEWIDTH]);
			this._maxUploadImageHeight = int.Parse(data[TableSpec.FIELD_MAXUPLOADIMAGEHEIGHT]);
		}

		private static Dictionary<int, int?> accountid2id = new Dictionary<int, int?>();
		public static IUserSettings LoadByAccount(Account account) {
			if(!accountid2id.ContainsKey(account.id)) {
				lock(accountid2id) {
					if(!accountid2id.ContainsKey(account.id)) {
						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComparisonCondition(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
								ComparisonType.EQUAL,
								account.id.ToString()
							),
							Diapasone.unlimited
						);
						if(ids.Count > 1) {
							throw new CriticalException("not unique");
						} else if(ids.Count == 1) {
							accountid2id[account.id] = int.Parse(ids[0]);
						} else {
							accountid2id[account.id] = null;
						}
					}
				}
			}
			if(accountid2id[account.id].HasValue) {
				return AccountSettings.LoadById(accountid2id[account.id].Value);
			} else {
				return new AnonymousUserSettings(account);
				//If cache for this account was just cleared, we will return AnonymousUserSettings. It is ok.
			}
		}
		private static void ClearCacheByAccount(Account account) {
			lock(accountid2id) {
				accountid2id.Remove(account.id);
			}
		}

		public static void Save(Account account, int postsPerPage, int threadsPerPage, int usersPerPage, int uploadsPerPage, Skin skin, ModernSkin modernSkin, Machichara machichara, int maxUploadImageWidth, int maxUploadImageHeight) {
			Dictionary<string, AbstractFieldValue> dataToUpdate = new Dictionary<string,AbstractFieldValue> {
				{ TableSpec.FIELD_POSTSPERPAGE, new ScalarFieldValue(postsPerPage.ToString()) },
				{ TableSpec.FIELD_THREADSPERPAGE, new ScalarFieldValue(threadsPerPage.ToString()) },
				{ TableSpec.FIELD_USERSPERPAGE, new ScalarFieldValue(usersPerPage.ToString()) },
				{ TableSpec.FIELD_UPLOADSPERPAGE, new ScalarFieldValue(uploadsPerPage.ToString()) },
				{ TableSpec.FIELD_SKINID, new ScalarFieldValue(skin.id.ToString()) },
				{ TableSpec.FIELD_MODERNSKINID, new ScalarFieldValue(modernSkin.id.ToString()) },
				{ TableSpec.FIELD_MACHICHARAID, new ScalarFieldValue(machichara.id.ToString()) },
				{ TableSpec.FIELD_MAXUPLOADIMAGEWIDTH, new ScalarFieldValue(maxUploadImageWidth.ToString()) },
				{ TableSpec.FIELD_MAXUPLOADIMAGEHEIGHT, new ScalarFieldValue(maxUploadImageHeight.ToString()) },
			};
			Dictionary<string, AbstractFieldValue> dataToInsert = new Dictionary<string,AbstractFieldValue>(dataToUpdate) {
				{ TableSpec.FIELD_ACCOUNTID, new ScalarFieldValue(account.id.ToString()) },
			};
			ChangeSetUtil.ApplyChanges(
				new InsertOrUpdateChange(
					TableSpec.instance,
					dataToInsert,
					dataToUpdate,
					new ComparisonCondition(
						TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
						ComparisonType.EQUAL,
						account.id.ToString()
					)
				)
			);
			if(LoadByAccount(account) is AnonymousUserSettings) {
				ClearCacheByAccount(account);
			}
		}

	}
}
