using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

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
				this.Load();
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

		protected override void doFromHash(Dictionary<string, string> data) {
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
			this._postsPerPage = int.Parse(data[TableSpec.FIELD_POSTSPERPAGE]);
			this._threadsPerPage = int.Parse(data[TableSpec.FIELD_THREADSPERPAGE]);
			this._uploadsPerPage = int.Parse(data[TableSpec.FIELD_UPLOADSPERPAGE]);
			this._usersPerPage = int.Parse(data[TableSpec.FIELD_USERSPERPAGE]);
			this._boardsViewSettings = data[TableSpec.FIELD_BOARDSVIEWSETTINGS];
			this._skinId = int.Parse(data[TableSpec.FIELD_SKINID]);
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
							Diapasone.unlimited,
							new JoinSpec[0]
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
				return new AnonymousUserSettings();
			}
		}

	}
}
