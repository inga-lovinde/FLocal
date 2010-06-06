using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using FLocal.Core;
using FLocal.Core.DB;

namespace FLocal.MySQLConnector {

	class Connection : IDBConnection {

		private MySqlConnection connection;
		private string connectionString;

		private HashSet<Transaction> transactions;

		public Connection(string connectionString) {
			this.connection = new MySqlConnection(connectionString);
			this.connection.Open();
			this.connectionString = connectionString;
		}

		internal MySqlConnection createConnection() {
			MySqlConnection connection = new MySqlConnection(this.connectionString);
			return connection;
		}

		public List<Dictionary<string, string>> LoadByIds(ITableSpec table, List<string> ids) {
			using(MySqlCommand command = this.connection.CreateCommand()) {
				command.CommandType = System.Data.CommandType.Text;

				ParamsHolder paramsHolder = new ParamsHolder();
				List<string> placeholder = new List<string>();
				foreach(string id in ids) {
					placeholder.Add("@" + paramsHolder.Add(id));
				}

				command.CommandText = "SELECT * FROM " + table.compile() + " WHERE " + table.getIdSpec().compile() + " IN (" + string.Join(", ", placeholder.ToArray()) + ")";
				foreach(KeyValuePair<string, string> kvp in paramsHolder.data) {
					command.Parameters.AddWithValue(kvp.Key, kvp.Value);
				}

				Dictionary<string, Dictionary<string, string>> rawResult = new Dictionary<string, Dictionary<string, string>>();
				using(MySqlDataReader reader = command.ExecuteReader()) {
					while(reader.Read()) {
						Dictionary<string, string> row = new Dictionary<string,string>();
						for(int i=0; i<reader.FieldCount; i++) {
							row.Add(reader.GetName(i), reader.GetString(i));
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
		}

		public List<string> LoadIdsByConditions(ITableSpec table, FLocal.Core.DB.conditions.AbstractCondition conditions, Diapasone diapasone, JoinSpec[] joins, SortSpec[] sorts) {

			using(MySqlCommand command = this.connection.CreateCommand()) {

				command.CommandType = System.Data.CommandType.Text;

				var conditionsCompiled = ConditionCompiler.Compile(conditions);
				string queryConditions = conditionsCompiled.Key;
				ParamsHolder paramsHolder = conditionsCompiled.Value;

				string queryJoins = "";

				string querySorts = "";
				{
				}

				string queryMain = "FROM " + table.compile() + " " + queryJoins + " WHERE " + queryConditions;

				foreach(KeyValuePair<string, string> kvp in paramsHolder.data) {
					command.Parameters.AddWithValue(kvp.Key, kvp.Value);
				}

				command.CommandText = "SELECT COUNT(*) " + queryMain;
				object rawCount = command.ExecuteScalar();
				int count = (int)rawCount;
				if(count < 1) {
					diapasone.total = 0;
					return new List<string>();
				} else {
					diapasone.total = count;
					string queryLimits = "";
					if(diapasone.count >= 0) {
						queryLimits = "LIMIT " + diapasone.count + " OFFSET " + diapasone.start;
					}
					command.CommandText = "SELECT " + table.compile() + ".* " + queryMain + " " + querySorts + " " + queryLimits;

					List<string> result = new List<string>();
					using(MySqlDataReader reader = command.ExecuteReader()) {
						while(reader.Read()) {
							result.Add(reader.GetString(0));
						}
					}
					return result;
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
				using(MySqlCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "LOCK TABLES `" + MySqlHelper.EscapeString(table.name) + "`";
					command.ExecuteNonQuery();
				}
			}
		}

		public void lockRow(FLocal.Core.DB.Transaction _transaction, ITableSpec table, string id) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(MySqlCommand command = transaction.sqlconnection.CreateCommand()) {
					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "SELECT * FROM `" + MySqlHelper.EscapeString(table.name) + "` where `" + MySqlHelper.EscapeString(table.idName) + "` = @id FOR UPDATE";
					command.Parameters.Add(new MySqlParameter("id", id));
					command.ExecuteNonQuery();
				}
			}
		}

		public void update(FLocal.Core.DB.Transaction _transaction, ITableSpec table, string id, Dictionary<string, string> data) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(MySqlCommand command = transaction.sqlconnection.CreateCommand()) {
					List<string> updates = new List<string>();
					ParamsHolder paramsholder = new ParamsHolder();
					foreach(KeyValuePair<string, string> kvp in data) {
						string paramname = paramsholder.Add(kvp.Value);
						updates.Add("`" + MySqlHelper.EscapeString(kvp.Key) + "` = @" + paramname);
					}

					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "UPDATE `" + MySqlHelper.EscapeString(table.name) + "` set " + String.Join(", ", updates.ToArray()) + " where `" + MySqlHelper.EscapeString(table.idName) + "` = @id";
					command.Parameters.AddWithValue("id", id);
					foreach(KeyValuePair<string, string> kvp in paramsholder.data) {
						command.Parameters.AddWithValue(kvp.Key, kvp.Value);
					}
					command.ExecuteNonQuery();
				}
			}
		}

		public string insert(FLocal.Core.DB.Transaction _transaction, ITableSpec table, Dictionary<string, string> data) {
			Transaction transaction = (Transaction)_transaction;
			lock(transaction) {
				using(MySqlCommand command = transaction.sqlconnection.CreateCommand()) {
					List<string> updates = new List<string>();
					ParamsHolder paramsholder = new ParamsHolder();
					foreach(KeyValuePair<string, string> kvp in data) {
						string paramname = paramsholder.Add(kvp.Value);
						updates.Add("`" + MySqlHelper.EscapeString(kvp.Key) + "` = @" + paramname);
					}

					command.Transaction = transaction.sqltransaction;
					command.CommandType = System.Data.CommandType.Text;
					command.CommandText = "INSERT INTO `" + MySqlHelper.EscapeString(table.name) + "` SET " + String.Join(", ", updates.ToArray());
					foreach(KeyValuePair<string, string> kvp in paramsholder.data) {
						command.Parameters.AddWithValue(kvp.Key, kvp.Value);
					}
					command.ExecuteNonQuery();
					return command.LastInsertedId.ToString();
				}
			}
		}

		public void delete(FLocal.Core.DB.Transaction transaction, ITableSpec table, string id) {
			throw new NotImplementedException();
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
				this.connection.Close();
				this.connection.Dispose();
				this.connection = null;
			}
		}

	}

}
