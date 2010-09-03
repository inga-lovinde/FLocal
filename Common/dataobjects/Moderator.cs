using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Core.DB;
using FLocal.Core.DB.conditions;

namespace FLocal.Common.dataobjects {
	public class Moderator : SqlObject<Moderator> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Moderators";
			public const string FIELD_ID = "Id";
			public const string FIELD_ACCOUNTID = "AccountId";
			public const string FIELD_BOARDID = "BoardId";
			public const string FIELD_ISACTIVE = "IsActive";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) {
				Refresh(id);
				var moderator = Moderator.LoadById(id);
				byAccount_Recalculate(moderator.accountId);
				byBoard_Recalculate(moderator.boardId);
				lock(isModerator_cache) {
					if(!isModerator_cache.ContainsKey(moderator.accountId)) isModerator_cache[moderator.accountId] = new Dictionary<int,bool>();
					isModerator_cache[moderator.accountId][moderator.boardId] = moderator.isActive;
				}
			}
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
				return Account.LoadById(this.accountId);
			}
		}

		private int _boardId;
		public int boardId {
			get {
				this.LoadIfNotLoaded();
				return this._boardId;
			}
		}
		public Board board {
			get {
				return Board.LoadById(this.boardId);
			}
		}

		private bool _isActive;
		public bool isActive {
			get {
				this.LoadIfNotLoaded();
				return this._isActive;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._accountId = int.Parse(data[TableSpec.FIELD_ACCOUNTID]);
			this._boardId = int.Parse(data[TableSpec.FIELD_BOARDID]);
			this._isActive = Util.string2bool(data[TableSpec.FIELD_ISACTIVE]);
		}

		private static readonly Dictionary<int, Dictionary<int, bool>> isModerator_cache = new Dictionary<int,Dictionary<int,bool>>();
		public static bool isTrueModerator(Account account, Board board) {
			//slight optimisation...
			UserGroup group = account.user.userGroup;
			if(group.name != UserGroup.NAME_ADMINISTRATORS && group.name != UserGroup.NAME_MODERATORS) return false;

			if(!isModerator_cache.ContainsKey(account.id) || !isModerator_cache[account.id].ContainsKey(board.id)) {
				lock(isModerator_cache) {
					if(!isModerator_cache.ContainsKey(account.id) || !isModerator_cache[account.id].ContainsKey(board.id)) {
						if(!isModerator_cache.ContainsKey(account.id)) isModerator_cache[account.id] = new Dictionary<int,bool>();

						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComplexCondition(
								ConditionsJoinType.AND,
								new ComparisonCondition(
									TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
									ComparisonType.EQUAL,
									account.id.ToString()
								),
								new ComparisonCondition(
									TableSpec.instance.getColumnSpec(TableSpec.FIELD_BOARDID),
									ComparisonType.EQUAL,
									board.id.ToString()
								)
							),
							Diapasone.unlimited
						);
						if(ids.Count < 1) {
							isModerator_cache[account.id][board.id] = false;
						} else {
							isModerator_cache[account.id][board.id] = Moderator.LoadById(int.Parse(ids.Single())).isActive;
						}

					}
				}
			}
			return isModerator_cache[account.id][board.id];
		}
		public static bool isModerator(Account account, Thread thread) {
			return (thread.board.isTopicstarterModeration && thread.topicstarterId == account.userId) || isTrueModerator(account, thread.board);
		}
		public static bool isModerator(User user, Thread thread) {
			Account account;
			try {
				account = Account.LoadByUser(user);
			} catch(NotFoundInDBException) {
				return false;
			}
			return isModerator(account, thread);
		}

		private static readonly Dictionary<int, IEnumerable<int>> byBoard_cache = new Dictionary<int,IEnumerable<int>>();
		private static void byBoard_Recalculate(int boardId) {
			lock(byBoard_cache) {
				byBoard_cache[boardId] = 
					from stringId in Config.instance.mainConnection.LoadIdsByConditions(
						TableSpec.instance,
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_BOARDID),
							ComparisonType.EQUAL,
							boardId.ToString()
						),
						Diapasone.unlimited
					)
					select Moderator.LoadById(int.Parse(stringId)).accountId;
			}
		}
		public static IEnumerable<Account> GetModerators(Board board) {
			if(!byBoard_cache.ContainsKey(board.id)) {
				byBoard_Recalculate(board.id);
			}
			return from id in byBoard_cache[board.id] select Account.LoadById(id);
		}

		private static readonly Dictionary<int, IEnumerable<int>> byAccount_cache = new Dictionary<int,IEnumerable<int>>();
		private static void byAccount_Recalculate(int accountId) {
			lock(byAccount_cache) {
				byAccount_cache[accountId] = 
					from stringId in Config.instance.mainConnection.LoadIdsByConditions(
						TableSpec.instance,
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_ACCOUNTID),
							ComparisonType.EQUAL,
							accountId.ToString()
						),
						Diapasone.unlimited
					)
					select Moderator.LoadById(int.Parse(stringId)).boardId;
			}
		}
		public static IEnumerable<Board> GetModeratedBoards(Account account) {
			if(!byAccount_cache.ContainsKey(account.id)) {
				byAccount_Recalculate(account.id);
			}
			return from id in byAccount_cache[account.id] select Board.LoadById(id);
		}

	}
}
