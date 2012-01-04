using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;

namespace Patcher.DB {

	class PostgresDBTraits : IDBTraits {

		private static DbDataReader ExecuteReader(DbCommand command) {
			Logger.instance.Log(command.CommandText);
			return command.ExecuteReader();
		}

		private static int ExecuteNonQuery(DbCommand command) {
			Logger.instance.Log(command.CommandText);
			return command.ExecuteNonQuery();
		}

		public static readonly IDBTraits instance = new PostgresDBTraits();

		protected PostgresDBTraits() {
		}

		private static readonly Regex ALPHANUMERIC = new Regex("^[a-zA-Z]\\w*$", RegexOptions.Compiled | RegexOptions.Singleline);

		private static string _EscapeName(string name) {
			if(!ALPHANUMERIC.IsMatch(name)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return string.Format("\"{0}\"", name);
		}

		string IDBTraits.EscapeName(string name) {
			return _EscapeName(name);
		}

		DbConnection IDBTraits.CreateConnection(string connectionString) {
			NpgsqlConnection connection = new NpgsqlConnection(connectionString);
			connection.Open();
			return connection;
		}

		string IDBTraits.MarkParam(string paramName) {
			if(!ALPHANUMERIC.IsMatch(paramName)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return ":" + paramName;
		}

		string IDBTraits.ParamName(string paramName) {
			if(!ALPHANUMERIC.IsMatch(paramName)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return paramName;
		}

		private static void AddParam(DbCommand command, string name, DbType type, object value) {
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.DbType = type;
			param.Value = value;
			command.Parameters.Add(param);
		}

		private static bool ParseBoolString(string value) {
			switch(value.ToLower()) {
				case "n":
					return false;
				case "y":
					return true;
				default:
					throw new ApplicationException(string.Format("Unknown value {0}", value));
			}
		}

		private static string ParseTypeString(string type) {
			switch(type.ToLower()) {
				case "int4":
					return "integer";
				default:
					return type;
			}
		}

		private static T CastResult<T>(object value) where T : class {
			if(DBNull.Value.Equals(value)) {
				return null;
			} else {
				return (T)value;
			}
		}

		private static T? CastScalarResult<T>(object value) where T : struct {
			if(DBNull.Value.Equals(value)) {
				return null;
			} else {
				return (T)value;
			}
		}

		private static readonly SQLQueryManager _SQLQueryManager = new SQLQueryManager(_EscapeName);

		StoredProcedureBody IDBTraits.GetStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure) {
			throw new NotImplementedException();
		}

		private static string FormatStoredProcedureHeader(StoredProcedureReference procedure) {
			throw new NotImplementedException();
		}

		void IDBTraits.ReplaceStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody newBody) {
			throw new NotImplementedException();
		}

		void IDBTraits.RemoveStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure) {
			throw new NotImplementedException();
		}

		void IDBTraits.CreateView(Func<DbCommand> commandCreator, string viewName, string body) {
			throw new NotImplementedException();
		}

		void IDBTraits.RemoveView(Func<DbCommand> commandCreator, string viewName) {
			throw new NotImplementedException();
		}

		void IDBTraits.CreateStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody body) {
			throw new NotImplementedException();
		}

		string IDBTraits.GetViewBody(Func<DbCommand> commandCreator, string name) {
			throw new NotImplementedException();
		}

		ColumnOptions IDBTraits.GetColumnOptions(Func<DbCommand> commandCreator, ColumnReference column) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = "SELECT a.attnum, a.attname AS field, t.typname AS type, a.attlen AS length, a.atttypmod AS lengthvar, a.attnotnull AS notnull, d.adsrc AS defaultvalue FROM pg_attribute a JOIN pg_class c ON a.attrelid = c.oid JOIN pg_type t ON a.atttypid = t.oid LEFT JOIN pg_attrdef d ON c.oid = d.adrelid AND a.attnum = d.adnum WHERE a.attnum > 0 AND c.relname = :ptable and a.attname = :pcolumn";
				AddParam(command, "ptable", DbType.String, column.tableName);
				AddParam(command, "pcolumn", DbType.String, column.columnName);
				using(DbDataReader reader = ExecuteReader(command)) {
					if(!reader.Read()) {
						throw new ApplicationException("Column not found");
					}

					return new ColumnOptions(
						ParseTypeString(reader.GetString(reader.GetOrdinal("type"))),
						reader.GetString(reader.GetOrdinal("defaultvalue")),
						reader.GetBoolean(reader.GetOrdinal("notnull"))
					);
				}
			}
		}

		void IDBTraits.RemoveColumn(Func<DbCommand> commandCreator, ColumnReference column) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.RemoveColumn(column);
				ExecuteNonQuery(command);
			}
		}

		void IDBTraits.CreateColumn(Func<DbCommand> commandCreator, ColumnDescription description) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateColumn(description);
				ExecuteNonQuery(command);
			}
		}

		void IDBTraits.ModifyColumn(Func<DbCommand> commandCreator, ColumnDescription description) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.ModifyColumnPostgresStyle(description);
				ExecuteNonQuery(command);
			}
		}

		private static string GetStringRepresentation(ForeignKeyConstraint.ReferentialAction action) {
			switch(action) {
				case ForeignKeyConstraint.ReferentialAction.NoAction:
					return "NO ACTION";
				case ForeignKeyConstraint.ReferentialAction.Cascade:
					return "CASCADE";
				case ForeignKeyConstraint.ReferentialAction.SetNull:
					return "SET NULL";
				case ForeignKeyConstraint.ReferentialAction.SetDefault:
					return "SET DEFAULT";
				default:
					throw new ApplicationException("Unknown referential action");
			}
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, ForeignKeyConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = string.Format("\\d {0}", _EscapeName(constraint.name));
				using(var reader = ExecuteReader(command)) {
					int row = 0;
					while(reader.Read()) {
						Logger.instance.Log("Row #" + row);
						for(int j=0; j<reader.FieldCount; j++) {
							Logger.instance.Log(reader.GetName(j) + "='" + reader.GetValue(j) + "'");
						}
						row++;
					}
				}
			}
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, UniqueConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = string.Format("\\d {0}", _EscapeName(constraint.name));
				using(var reader = ExecuteReader(command)) {
					int row = 0;
					while(reader.Read()) {
						Logger.instance.Log("Row #" + row);
						for(int j=0; j<reader.FieldCount; j++) {
							Logger.instance.Log(reader.GetName(j) + "='" + reader.GetValue(j) + "'");
						}
						row++;
					}
				}
			}
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, CheckConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = string.Format("\\d {0}", _EscapeName(constraint.name));
				using(var reader = ExecuteReader(command)) {
					int row = 0;
					while(reader.Read()) {
						Logger.instance.Log("Row #" + row);
						for(int j=0; j<reader.FieldCount; j++) {
							Logger.instance.Log(reader.GetName(j) + "='" + reader.GetValue(j) + "'");
						}
						row++;
					}
				}
			}
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			constraint.Accept(fkc => CheckConstraint(commandCreator, fkc), uc => CheckConstraint(commandCreator, uc), cc => CheckConstraint(commandCreator, cc));
		}

		public void RemoveConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			CheckConstraint(commandCreator, constraint);
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.DropConstraint(constraint);
				ExecuteNonQuery(command);
			}
		}

		public void CreateConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateConstraint(constraint);
				ExecuteNonQuery(command);
			}
		}

		public void CreateTable(Func<DbCommand> commandCreator, TableDescription table) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateTable(table);
				ExecuteNonQuery(command);
			}
		}

		private void CheckTable(Func<DbCommand> commandCreator, TableDescription table) {
			HashSet<string> columns = new HashSet<string>(from column in table.columns select column.column.columnName);
			columns.Add(table.primaryKey.column.columnName);

			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT attname FROM pg_attribute, pg_class WHERE pg_class.oid = attrelid AND attnum>0 AND relname = ':ptable'";
				AddParam(command, "ptable", DbType.String, table.table);
				
				using(var reader = ExecuteReader(command))
				{
					HashSet<string> dbColumns = new HashSet<string>();
					while(reader.Read())
					{
						dbColumns.Add(reader.GetValue("attname").ToString());
					}
					
					if(!dbColumns.IsSubsetOf(columns))
					{
						throw new FormattableException("Some columns are not mentioned in table definition: {0}", string.Join(",", dbColumns.Except(columns).ToArray()));
					}
					if(!dbColumns.IsSupersetOf(columns))
					{
						throw new FormattableException("Some columns are missed in DB: {0}", string.Join(",", columns.Except(dbColumns).ToArray()));
					}
				}
			}

			var options = (this as IDBTraits).GetColumnOptions(commandCreator, table.primaryKey.column);
			/*Console.WriteLine();
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.type, options.type);
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.defaultValue, options.defaultValue);
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.isNotNull, options.isNotNull);*/
			if(!table.primaryKey.options.Equals((this as IDBTraits).GetColumnOptions(commandCreator, table.primaryKey.column))) {
				throw new FormattableException("Column {0} definition mismatch", table.primaryKey.column.columnName);
			}

			foreach(var column in table.columns) {
				options = (this as IDBTraits).GetColumnOptions(commandCreator, column.column);
				/*Console.WriteLine();
				Console.WriteLine("'{0}' vs '{1}'", column.options.type, options.type);
				Console.WriteLine("'{0}' vs '{1}'", column.options.defaultValue, options.defaultValue);
				Console.WriteLine("'{0}' vs '{1}'", column.options.isNotNull, options.isNotNull);*/
				if(!column.options.Equals((this as IDBTraits).GetColumnOptions(commandCreator, column.column))) {
					throw new FormattableException("Column {0} definition mismatch", column.column.columnName);
				}
			}
		}

		void IDBTraits.RemoveTable(Func<DbCommand> commandCreator, TableDescription table) {
			this.CheckTable(commandCreator, table);
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.DropTable(table.table);
				ExecuteNonQuery(command);
			}
		}

		bool IDBTraits.IsDDLTransactional {
			get { return true; }
		}

	}
}
