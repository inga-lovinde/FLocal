using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.IO;

namespace Patcher.DB
{
	interface IDBTraits
	{

		string EscapeName(string name);

		string MarkParam(string rawParamName);

		string ParamName(string rawParamName);

		DbConnection CreateConnection(string connectionString);

		StoredProcedureBody GetStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure);
		void ReplaceStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody newBody);
		void RemoveStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure);
		void CreateStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody body);

		ColumnOptions GetColumnOptions(Func<DbCommand> commandCreator, ColumnReference column);
		void RemoveColumn(Func<DbCommand> commandCreator, ColumnReference column);
		void CreateColumn(Func<DbCommand> commandCreator, ColumnDescription description);
		void ModifyColumn(Func<DbCommand> commandCreator, ColumnDescription description);

		void RemoveConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint);
		void CreateConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint);

		void CreateTable(Func<DbCommand> commandCreator, TableDescription table);
		void RemoveTable(Func<DbCommand> commandCreator, TableDescription table);

		string GetViewBody(Func<DbCommand> commandCreator, string viewName);
		void CreateView(Func<DbCommand> commandCreator, string viewName, string body);
		void RemoveView(Func<DbCommand> commandCreator, string viewName);
		
		bool IsDDLTransactional
		{ get; }
		
	}
}
