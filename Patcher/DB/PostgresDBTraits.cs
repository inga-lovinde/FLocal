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
			throw new NotImplementedException();
		}

		void IDBTraits.RemoveColumn(Func<DbCommand> commandCreator, ColumnReference column) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.RemoveColumn(column);
				command.ExecuteNonQuery();
			}
		}

		void IDBTraits.CreateColumn(Func<DbCommand> commandCreator, ColumnDescription description) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateColumn(description);
				command.ExecuteNonQuery();
			}
		}

		void IDBTraits.ModifyColumn(Func<DbCommand> commandCreator, ColumnDescription description) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.ModifyColumnPostgresStyle(description);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
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
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, UniqueConstraint constraint) {
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, CheckConstraint constraint) {
			throw new NotImplementedException();
		}

		private void CheckConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			constraint.Accept(fkc => CheckConstraint(commandCreator, fkc), uc => CheckConstraint(commandCreator, uc), cc => CheckConstraint(commandCreator, cc));
		}

		public void RemoveConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			CheckConstraint(commandCreator, constraint);
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.DropConstraint(constraint);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public void CreateConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateConstraint(constraint);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public void CreateTable(Func<DbCommand> commandCreator, TableDescription table) {
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.CreateTable(table);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		private void CheckTable(Func<DbCommand> commandCreator, TableDescription table) {
			throw new NotImplementedException();
		}

		void IDBTraits.RemoveTable(Func<DbCommand> commandCreator, TableDescription table) {
			this.CheckTable(commandCreator, table);
			using(DbCommand command = commandCreator()) {
				command.CommandText = _SQLQueryManager.DropTable(table.table);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		bool IDBTraits.IsDDLTransactional {
			get { return true; }
		}

	}
}
