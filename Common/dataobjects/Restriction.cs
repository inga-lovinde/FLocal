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
	public class Restriction : SqlObject<Restriction> {

		public class TableSpec : ISqlObjectTableSpec {
			public const string TABLE = "Restrictions";
			public const string FIELD_ID = "Id";
			public const string FIELD_USERID = "UserId";
			public const string FIELD_BOARDID = "BoardId";
			public const string FIELD_DATA = "Data";
			public static readonly TableSpec instance = new TableSpec();
			public string name { get { return TABLE; } }
			public string idName { get { return FIELD_ID; } }
			public void refreshSqlObject(int id) {
				Refresh(id);
				var restriction = Restriction.LoadById(id);
				byUser_Recalculate(restriction.userId);
				byBoard_Recalculate(restriction.boardId);
				lock(restrictionId_cache) {
					if(!restrictionId_cache.ContainsKey(restriction.userId)) restrictionId_cache[restriction.userId] = new Dictionary<int, int?>();
					restrictionId_cache[restriction.userId][restriction.boardId] = restriction.id;
				}
			}
		}

		protected override ISqlObjectTableSpec table { get { return TableSpec.instance; } }

		private int _userId;
		public int userId {
			get {
				this.LoadIfNotLoaded();
				return this._userId;
			}
		}
		public User user {
			get {
				return User.LoadById(this.userId);
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

		private Dictionary<int, DateTime> _data;
		public Dictionary<int, DateTime> data {
			get {
				this.LoadIfNotLoaded();
				return this._data;
			}
		}

		protected override void doFromHash(Dictionary<string, string> data) {
			this._userId = int.Parse(data[TableSpec.FIELD_USERID]);
			this._boardId = int.Parse(data[TableSpec.FIELD_BOARDID]);
			this._data = (
				from part in data[TableSpec.FIELD_DATA].Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)
				let subparts = part.Split(new char[] {':'}, 2)
				select new KeyValuePair<int, DateTime>(
					int.Parse(subparts[0]),
					new DateTime(long.Parse(subparts[1]))
				)
			).ToDictionary();
		}

		private static readonly Dictionary<int, Dictionary<int, int?>> restrictionId_cache = new Dictionary<int,Dictionary<int, int?>>();
		public static Restriction GetRestriction(User user, Board board) {

			if(!restrictionId_cache.ContainsKey(user.id) || !restrictionId_cache[user.id].ContainsKey(board.id)) {
				lock(restrictionId_cache) {
					if(!restrictionId_cache.ContainsKey(user.id) || !restrictionId_cache[user.id].ContainsKey(board.id)) {
						if(!restrictionId_cache.ContainsKey(user.id)) restrictionId_cache[user.id] = new Dictionary<int, int?>();

						List<string> ids = Config.instance.mainConnection.LoadIdsByConditions(
							TableSpec.instance,
							new ComplexCondition(
								ConditionsJoinType.AND,
								new ComparisonCondition(
									TableSpec.instance.getColumnSpec(TableSpec.FIELD_USERID),
									ComparisonType.EQUAL,
									user.id.ToString()
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
							restrictionId_cache[user.id][board.id] = null;
						} else {
							restrictionId_cache[user.id][board.id] = Restriction.LoadById(int.Parse(ids.Single())).id;
						}

					}
				}
			}

			if(restrictionId_cache[user.id][board.id].HasValue) {
				return Restriction.LoadById(restrictionId_cache[user.id][board.id].Value);
			} else {
				return null;
			}
		}

		public static Dictionary<int, DateTime> GetRestrictionData(User user, Board board) {
			Restriction restriction = GetRestriction(user, board);
			if(restriction != null) {
				return restriction.data;
			} else {
				return new Dictionary<int,DateTime>();
			}
		}

		private static readonly Dictionary<int, IEnumerable<int>> byBoard_cache = new Dictionary<int, IEnumerable<int>>();
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
					select Restriction.LoadById(int.Parse(stringId)).id;
			}
		}
		public static IEnumerable<User> GetRestrictedUsers(Board board) {
			if(!byBoard_cache.ContainsKey(board.id)) {
				byBoard_Recalculate(board.id);
			}
			return from id in byBoard_cache[board.id] select Restriction.LoadById(id).user;
		}

		private static readonly Dictionary<int, IEnumerable<int>> byUser_cache = new Dictionary<int,IEnumerable<int>>();
		private static void byUser_Recalculate(int userId) {
			lock(byUser_cache) {
				byUser_cache[userId] = 
					from stringId in Config.instance.mainConnection.LoadIdsByConditions(
						TableSpec.instance,
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_USERID),
							ComparisonType.EQUAL,
							userId.ToString()
						),
						Diapasone.unlimited
					)
					select Restriction.LoadById(int.Parse(stringId)).id;
			}
		}
		public static IEnumerable<Board> GetRestrictions(User user) {
			if(!byUser_cache.ContainsKey(user.id)) {
				byUser_Recalculate(user.id);
			}
			return from id in byUser_cache[user.id] select Restriction.LoadById(id).board;
		}

		public static void RecalculateRestrictions(Board board, User user) {
			List<Punishment> punishments = (from punishment in Punishment.getEffectivePunishments(user, board) where punishment.punishmentType.weight > 0 orderby punishment.expires descending select punishment).ToList();

			Dictionary<int, DateTime> layer2expirationDate = new Dictionary<int,DateTime>();
			foreach(var layer in PostLayer.allLayers) {

				if(!layer.maxPunishments.HasValue) continue;

				bool restricted = false;
				int accumulated = 0;
				DateTime? expirationDate = null;
				foreach(var punishment in punishments) {
					accumulated += punishment.punishmentType.weight;
					if(accumulated >= layer.maxPunishments.Value) {
						expirationDate = punishment.expires;
						restricted = true;
						break;
					}
				}

				if(restricted) {
					layer2expirationDate[layer.id] = expirationDate.Value;
				}
			}

			string data = (from kvp in layer2expirationDate select string.Format("{0}:{1}", kvp.Key, kvp.Value.Ticks)).Join(";");

			ChangeSetUtil.ApplyChanges(
				new InsertOrUpdateChange(
					TableSpec.instance,
					new Dictionary<string,AbstractFieldValue> {
						{ TableSpec.FIELD_BOARDID, new ScalarFieldValue(board.id.ToString()) },
						{ TableSpec.FIELD_USERID, new ScalarFieldValue(user.id.ToString()) },
						{ TableSpec.FIELD_DATA, new ScalarFieldValue(data) },
					},
					new Dictionary<string,AbstractFieldValue> {
						{ TableSpec.FIELD_DATA, new ScalarFieldValue(data) },
					},
					new ComplexCondition(
						ConditionsJoinType.AND,
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_BOARDID),
							ComparisonType.EQUAL,
							board.id.ToString()
						),
						new ComparisonCondition(
							TableSpec.instance.getColumnSpec(TableSpec.FIELD_USERID),
							ComparisonType.EQUAL,
							user.id.ToString()
						)
					)
				)
			);
		}

	}
}
