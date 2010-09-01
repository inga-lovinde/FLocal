using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.MySQLConnector {

	public class Connection : IDBConnection {

		internal readonly IDBTraits traits;

//		private DbConnection connection;
		private string connectionString;

		private HashSet<Transaction> transactions;

		public Connection(string connectionString, IDBTraits traits) {
			this.traits = traits;
			this.connectionString = connectionString;
			using(DbConnection connection = this.createConnection()) {
				//just testing we can open a connection
			}
			this.transactions = new HashSet<Transaction>();
		}

		internal DbConnection createConnection() {
			DbConnection connection = this.traits.createConnection(this.connectionString);
			connection.Open();
			return connection;
		}

		private List<Dictionary<string, string>> _LoadByIds(DbCommand command, ITableSpec table, List<string> ids, bool forUpdate) {
			command.CommandType = System.Data.CommandType.Text;

			ParamsHolder paramsHolder = new ParamsHolder();
			List<string> placeholder = new List<string>();
			foreach(string id in ids) {
				placeholder.Add(this.traits.markParam(paramsHolder.Add(id)));
			}

			command.CommandText = "SELECT * FROM " + table.compile(this.traits) + " WHERE " + table.getIdSpec().compile(this.traits) + " IN (" + string.Join(", ", placeholder.ToArray()) + ")" + (forUpdate ? " FOR UPDATE" : "");
			//command.Prepare();
			foreach(KeyValuePair<string, string> kvp in paramsHolder.data) {
				command.AddParameter(kvp.Key, kvp.Value);
			}

			Dictionary<string, Dictionary<string, string>> rawResult = new Dictionary<string, Dictionary<string, string>>();
			using(DbDataReader reader = command.ExecuteReader()) {
				while(reader.Read()) {
					Dictionary<string, string> row = new Dictionary<string,string>();
					for(int i=0; i<reader.FieldCount; i++) {
//								throw new CriticalException("Name: " + reader.GetName(i));
						object value = reader.GetValue(i);
						string sValue;
						if(value is DateTime) {
							sValue = ((DateTime)value).Ticks.ToString();
						} else if(value is TimeSpan) {
							sValue = ((TimeSpan)value).Ticks.ToString();
						} else {
							sValue = value.ToString();
						}
						row.Add(reader.GetName(i), sValue);
					}
					rawResult.Add(row[table.idName], row);
				}
			}

			List<Dictionary<string, string>> result = new List<Dictionary<string,string>>();
			foreach(string id in ids) {
				if(rawResult.ContainsKey(id)) {
					result.Add(rawResult[id]);
				}
			}
			return result;
		}

		public List<Dictionary<string, string>> LoadByIds(ITableSpec table, List<string> ids) {
			using(DbConnection connection = this.createConnection()) {
				using(DbCommand command = connection.CreateCommand()) {
					return this._LoadByIds(command, table, ids, false);
				}
			}
		}

		private List<string> _LoadIdsByConditions(DbCommand command, ITableSpec table, FLocal.Core.DB.conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts, bool allowHugeLists) {
			command.CommandType = System.Data.CommandType.Text;

			var conditionsCompiled = ConditionCompiler.Compile(conditions, this.traits);
			string queryConditions = "";
			if(conditionsCompiled.Key != "") queryConditions = "WHERE " + conditionsCompiled.Key;
			ParamsHolder paramsHolder = conditionsCompiled.Value;

			StringBuilder queryJoins = new StringBuilder();
			{
				foreach(var join in joins) {
					queryJoins.Append(" JOIN ");
					queryJoins.Append(join.additionalTableRaw.compile(this.traits));
					queryJoins.Append(" ");
					queryJoins.Append(join.additionalTable.compile(this.traits));
					queryJoins.Append(" ON ");
					queryJoins.Append(join.mainColumn.compile(this.traits));
					queryJoins.Append(" = ");
					queryJoins.Append(join.additionalTable.getIdSpec().compile(this.traits));
				}
			}

			string querySorts = "";
			if(sorts.Length > 0) {
				List<string> sortParts = new List<string>();
				foreach(SortSpec sortSpec in sorts) {
					if(sortSpec.ascending) {
						sortParts.Add(sortSpec.column.compile(this.traits) + " ASC");
					} else {
						sortParts.Add(sortSpec.column.compile(this.traits) + " DESC");
					}
				}
				querySorts = "ORDER BY " + string.Join(", ", sortParts.ToArray());
			}

			string queryMain = "FROM " + table.compile(this.traits) + " " + queryJoins + " " + queryConditions;

			foreach(KeyValuePair<string, string> kvp in paramsHolder.data) {
				command.AddParameter(kvp.Key, kvp.Value);
			}

			command.CommandText = "SELECT COUNT(*) " + queryMain;
			object rawCount;
			//try {
				rawCount = command.ExecuteScalar();
			//} catch(Npgsql.NpgsqlException e) {
				//throw new FLocalException("Error while trying to execute " + command.CommandText + ": " + e.Message);
			//}
			long count = (long)rawCount;
			if(count < 1) {
				diapasone.total = 0;
				return new List<string>();
			} else {
				diapasone.total = count;
				if(diapasone.total > 1000 && diapasone.count < 0 && !allowHugeLists) {
					throw new CriticalException("huge list");
				}
				string queryLimits = "";
				if(diapasone.count >= 0) {
					queryLimits = "LIMIT " + diapasone.count + " OFFSET " + diapasone.start;
				}
				command.CommandText = "SELECT " + table.getIdSpec().compile(this.traits) + " " + queryMain + " " + querySorts + " " + queryLimits;

				List<string> result = new List<string>();
				using(DbDataReader reader = command.ExecuteReader()) {
					while(reader.Read()) {
						result.Add(reader.GetValue(0).ToString());
					}
				}
				return result;
			}
		}

		public List<string> LoadIdsByConditions(ITableSpec table, FLocal.Core.DB.conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts, bool allowHugeLists) {
			using(DbConnection connection = this.createConnection()) {
				using(DbCommand command = connection.CreateCommand()) {
					return this._LoadIdsByConditions(command, table, conditions, diapasone, joins, sorts, allowHugeLists);
				}
			}
		}

		public long GetCountByConditions(ITableSpec table, FLocal.Core.DB.conditions.AbstractCondition conditions, params JoinSpec[] joins) {
			using(DbConnection connection = this.createConnection()) {
				using(DbCommand command = connection.CreateCommand()) {

					command.CommandType = System.Data.CommandType.Text;

					var conditionsCompiled = ConditionCompiler.Compile(conditions, this.traits);
					string queryConditions = "";
					if(conditionsCompiled.Key != "") queryConditions = "WHERE " + conditionsCompiled.Key;
					ParamsHolder paramsHolder = conditionsCompiled.Value;

					string queryJoins = "";
					{
						if(joins.Length > 0) {
							throw new NotImplementedException();
						}
					}


					command.CommandText = "SELECT COUNT(*) " + "FROM " + table.compile(this.traits) + " " + queryJoins + " " + queryConditions;
					foreach(KeyValuePair<string, string> kvp in paramsHolder.data) {
						command.AddParameter(kvp.Key, kvp.Value);
					}
					object rawCount = command.ExecuteScalar();
					long count = (long)rawCount;
					return count;
				}
			}
		}

		public FLocal.Core.DB.Transaction beginTransaction(System.Data.IsolationLevel iso) {
			lock(this) {
				Transaction transaction = new Transaction(this, iso);
				try {
					this.transactions.Add(transaction);
				} catch(Exception e) {
					transaction.Dispose();
					throw e;
				}
				return transaction;
			}
		}

		public void lockTable(FLocal.Core.DB.Transaction _transaction, ITableSpec table) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "LOCK TABLE " + table.compile(this.traits);
					command.ExecuteNonQuery();
				}
			}
		}

		public void lockRow(FLocal.Core.DB.Transaction _transaction, ITableSpec table, string id) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "SELECT * FROM " + table.compile(this.traits) + " where " + table.getIdSpec().compile(this.traits) + " = " + this.traits.markParam("id") + " FOR UPDATE";
					command.AddParameter("id", id);
					command.ExecuteNonQuery();
				}
			}
		}

		public List<Dictionary<string, string>> LoadByIds(FLocal.Core.DB.Transaction _transaction, ITableSpec table, List<string> ids) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					return this._LoadByIds(command, table, ids, true);
				}
			}
		}

		public List<string> LoadIdsByConditions(FLocal.Core.DB.Transaction _transaction, ITableSpec table, FLocal.Core.DB.conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts, bool allowHugeLists) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					return this._LoadIdsByConditions(command, table, conditions, diapasone, joins, sorts, allowHugeLists);
				}
			}
		}

		public void update(FLocal.Core.DB.Transaction _transaction, ITableSpec table, string id, Dictionary<string, string> data) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					List<string> updates = new List<string>();
					ParamsHolder paramsholder = new ParamsHolder();
					foreach(KeyValuePair<string, string> kvp in data) {
						updates.Add(this.traits.escapeIdentifier(kvp.Key) + " = " + this.traits.markParam(paramsholder.Add(kvp.Value)));
					}

					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "UPDATE " + table.compile(traits) + " set " + String.Join(", ", updates.ToArray()) + " where " + table.getIdSpec().compile(this.traits) + " = " + this.traits.markParam("id");
					command.AddParameter("id", id);
					foreach(KeyValuePair<string, string> kvp in paramsholder.data) {
						command.AddParameter(kvp.Key, kvp.Value);
					}
//					throw new CriticalException(command.CommandText + "; parameters: " + string.Join(", ", (from DbParameter parameter in command.Parameters select parameter.ParameterName + "='" + parameter.Value.ToString() + "'").ToArray()));
					command.ExecuteNonQuery();
				}
			}
		}

		public string insert(FLocal.Core.DB.Transaction _transaction, ITableSpec table, Dictionary<string, string> data) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					List<string> updates = new List<string>();
					List<string> updatesPlaceholders = new List<string>();
					ParamsHolder paramsholder = new ParamsHolder();
					foreach(KeyValuePair<string, string> kvp in data) {
						updates.Add(this.traits.escapeIdentifier(kvp.Key));
						updatesPlaceholders.Add(this.traits.markParam(paramsholder.Add(kvp.Value)));
					}

					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "INSERT INTO " + table.compile(this.traits) + " (" + String.Join(", ", updates.ToArray()) + ") VALUES (" + String.Join(", ", updatesPlaceholders.ToArray()) + ")";
					foreach(KeyValuePair<string, string> kvp in paramsholder.data) {
						command.AddParameter(kvp.Key, kvp.Value);
					}
					command.ExecuteNonQuery();
					if(data.ContainsKey(table.idName)) return data[table.idName];
					return this.traits.LastInsertId(command, table).ToString();
				}
			}
		}

		public void delete(FLocal.Core.DB.Transaction _transaction, ITableSpec table, string id) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(DbCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "DELETE FROM " + table.compile(traits) + " where " + table.getIdSpec().compile(this.traits) + " = " + this.traits.markParam("id");
					command.AddParameter("id", id);
					command.ExecuteNonQuery();
				}
			}
		}

		internal void RemoveTransaction(Transaction transaction) {
			lock(this) {
				this.transactions.Remove(transaction);
			}
		}

		public void Dispose() {
			lock(this) {
				foreach(Transaction transaction in this.transactions) {
					if(!transaction.finalizedImpl) {
						throw new CriticalException("Trying to close db connection while there are open transactions");
					}
				}
				this.transactions.Clear();
				this.transactions = null;
//				this.connection.Close();
//				this.connection.Dispose();
//				this.connection = null;
			}
		}

	}

}
