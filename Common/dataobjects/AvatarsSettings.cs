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
	public class AvatarsSettings : SqlObject<AvatarsSettings> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Accounts_AvatarsSettings";
			public const string FIELD_ID = "Id";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_AVATARS = "Avatars";
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
		
		private HashSet<int> _avatarsIds;
		public HashSet<int> avatarsIds {
			get {
				this.LoadIfNotLoaded();
				return new HashSet<int>(this._avatarsIds);
			}
		}
		public IEnumerable<Upload> avatars {
			get {
				return from avatarId in this.avatarsIds select Upload.LoadById(avatarId);
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
			this._avatarsIds = new HashSet<int>(from stringId in data[TableSpec.FIELD_AVATARS].Split(',') select int.Parse(stringId));
		}

		private static readonly Dictionary<int, int> accountid2id = new Dictionary<int,int>();
		public static AvatarsSettings LoadByAccount(Account account) {
			if(!accountid2id.ContainsKey(account.id)) {
				lock(accountid2id) {
					if(!accountid2id.ContainsKey(account.id)) {
						accountid2id[account.id] = int.Parse(
							Config.instance.mainConnection.LoadIdByField(
								TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
								account.id.ToString()
							)
						);
					}
				}
			}
			return AvatarsSettings.LoadById(accountid2id[account.id]);
		}

		private static void Save(Account account, HashSet<int> avatars) {
			Dictionary<string, AbstractFieldValue> dataToUpdate = new Dictionary<string,AbstractFieldValue> {
				{ TableSpec.FIELD_AVATARS, new ScalarFieldValue(avatars.Join(",")) },
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
		}

		private static HashSet<int> SafeGetAvatars(Account account) {
			try {
				return AvatarsSettings.LoadByAccount(account).avatarsIds;
			} catch(NotFoundInDBException) {
				return new HashSet<int>();
			}
		}

		public static void AddAvatar(Account account, Upload avatar) {

			if(avatar.size > Upload.AVATAR_MAX_FILESIZE) throw new FLocalException("Avatar is too big (max. 80KB allowed)");

			HashSet<int> avatars = SafeGetAvatars(account);
			avatars.Add(avatar.id);
			
			Save(account, avatars);
		}

		public static void RemoveAvatar(Account account, Upload avatar) {
			HashSet<int> avatars = SafeGetAvatars(account);
			avatars.Remove(avatar.id);

			Save(account, avatars);
		}

	}
}
