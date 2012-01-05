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
					return "a";
				case ForeignKeyConstraint.ReferentialAction.Cascade:
					return "c";
				case ForeignKeyConstraint.ReferentialAction.SetNull:
					return "n";
				case ForeignKeyConstraint.ReferentialAction.SetDefault:
					return "d";
				case ForeignKeyConstraint.ReferentialAction.Restrict:
					return "r";
				default:
					throw new ApplicationException("Unknown referential action");
			}
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, ForeignKeyConstraint constraint) {

			Int64 referencedTableOid;

			Int16[] attnums;
			Int16[] fattnums;

			using(DbCommand command = commandCreator()) {
				command.CommandText = "select c.* from pg_constraint c join pg_class r on c.conrelid = r.oid where r.relname = :ptable and c.conname = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);

				using(var reader = ExecuteReader(command)) {
					if(!reader.Read()) {
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}

					if(reader.GetValue("contype").ToString() != "f") {
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "C", reader.GetValue("contype"));
					}

					if(reader.GetValue("confdeltype").ToString() != GetStringRepresentation(constraint.onDelete)) {
						throw new FormattableException("Constraint {0}.{1} wrong on delete action: expected {2}, got {3}", constraint.table, constraint.name, GetStringRepresentation(constraint.onDelete), reader.GetValue("confdeltype"));
					}

					if(reader.GetValue("confupdtype").ToString() != GetStringRepresentation(constraint.onUpdate)) {
						throw new FormattableException("Constraint {0}.{1} wrong on update action: expected {2}, got {3}", constraint.table, constraint.name, GetStringRepresentation(constraint.onUpdate), reader.GetValue("confupdtype"));
					}

					attnums = (Int16[])reader.GetValue("conkey");
					fattnums = (Int16[])reader.GetValue("confkey");

					referencedTableOid = (Int64)reader.GetValue("confrelid");
				}

				if(attnums.Length != 1 || fattnums.Length != 1) {
					throw new FormattableException("More than one field participating in {0}.{1}", constraint.table, constraint.name);
				}

			}

			using(DbCommand command = commandCreator()) {
				command.CommandText = "select c.*, r.relname relname from pg_constraint c join pg_class r on c.conrelid = r.oid where r.oid = :prelid and c.contype = :ptype";
				AddParam(command, "prelid", DbType.Int64, referencedTableOid);
				AddParam(command, "ptype", DbType.String, "p");

				using(var reader = ExecuteReader(command)) {
					reader.Read();
					if(reader.GetValue("relname").ToString() != constraint.referencedTable) {
						throw new FormattableException("Constraint {0}.{1} wrong referenced table: expected {2}, got {3}", constraint.table, constraint.name, reader.GetValue("relname"), constraint.referencedTable);
					}

					Int16[] rattnums = (Int16[])reader.GetValue("conkey");
					if(rattnums.Length != 1) {
						throw new FormattableException("Referenced table {0} primary key consists of {1} columns", constraint.referencedTable, rattnums.Length);
					}

					if(rattnums[0] != fattnums[0]) {
						throw new FormattableException("Referenced column {2} for constraint {0}.{1} differs from column {3} used in a primary key of referenced table", constraint.table, constraint.name, fattnums[0], rattnums[0]);
					}
				}
			}

			using(DbCommand command = commandCreator()) {
				command.CommandText = "SELECT a.attnum, a.attname AS field, t.typname AS type, a.attlen AS length, a.atttypmod AS lengthvar, a.attnotnull AS notnull, d.adsrc AS defaultvalue FROM pg_attribute a JOIN pg_class c ON a.attrelid = c.oid JOIN pg_type t ON a.atttypid = t.oid LEFT JOIN pg_attrdef d ON c.oid = d.adrelid AND a.attnum = d.adnum WHERE c.relname = :ptable and a.attnum = :pattnum";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pattnum", DbType.Int16, attnums[0]);

				using(var reader = ExecuteReader(command)) {
					reader.Read();
					if(reader.GetValue("field").ToString() != constraint.column) {
						throw new FormattableException("Constraint {0}.{1} wrong column: expected {2}, got {3}", constraint.table, constraint.name, constraint.column, reader.GetValue("field"));
					}
				}
			}
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, UniqueConstraint constraint) {

			Int16[] attnums = null;

			using(DbCommand command = commandCreator()) {
				command.CommandText = "select c.* from pg_constraint c join pg_class r on c.conrelid = r.oid where r.relname = :ptable and c.conname = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);

				using(var reader = ExecuteReader(command)) {
					if(!reader.Read()) {
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}

					if(reader.GetValue("contype").ToString() != "u") {
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "C", reader.GetValue("contype"));
					}

					attnums = (Int16[])reader.GetValue("conkey");
				}
			}

			using(DbCommand command = commandCreator()) {
				command.CommandText = string.Format("SELECT a.attnum, a.attname AS field, t.typname AS type, a.attlen AS length, a.atttypmod AS lengthvar, a.attnotnull AS notnull, d.adsrc AS defaultvalue FROM pg_attribute a JOIN pg_class c ON a.attrelid = c.oid JOIN pg_type t ON a.atttypid = t.oid LEFT JOIN pg_attrdef d ON c.oid = d.adrelid AND a.attnum = d.adnum WHERE c.relname = :ptable and a.attnum in ({0})", string.Join(", ", (from i in Enumerable.Range(0, attnums.Length) select ":pattnum" + i).ToArray()));
				AddParam(command, "ptable", DbType.String, constraint.table);
				for(int i=0; i<attnums.Length; i++) {
					AddParam(command, "pattnum" + i, DbType.Int16, attnums[i]);
				}

				using(var reader = ExecuteReader(command)) {
					HashSet<string> dbColumns = new HashSet<string>();
					while(reader.Read()) {
						dbColumns.Add(reader.GetValue("field").ToString());
					}

					if(!dbColumns.IsSubsetOf(constraint.columns)) {
						throw new FormattableException("Some columns are not mentioned in constraint definition: {0}", string.Join(",", dbColumns.Except(constraint.columns).ToArray()));
					}
					if(!dbColumns.IsSupersetOf(constraint.columns)) {
						throw new FormattableException("Some columns are missed in DB: {0}", string.Join(",", constraint.columns.Except(dbColumns).ToArray()));
					}
				}
			}
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, CheckConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = "select c.* from pg_constraint c join pg_class r on c.conrelid = r.oid where r.relname = :ptable and c.conname = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				using(var reader = ExecuteReader(command)) {
					if(!reader.Read()) {
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}

					if(reader.GetValue("contype").ToString() != "c") {
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "C", reader.GetValue("contype"));
					}

					if(reader.GetValue("consrc").ToString() != constraint.condition) {
						throw new FormattableException("Constraint {0}.{1} wrong condition: expected {2}, got {3}", constraint.table, constraint.name, constraint.condition, reader.GetValue("consrc"));
					}
				}
			}
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
