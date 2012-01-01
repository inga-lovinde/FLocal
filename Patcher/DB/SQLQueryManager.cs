using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Patcher;

namespace Patcher.DB
{

	/// <summary>
	/// This class contains ANSI SQL query builders<br/>
	/// </summary>
	/// <see cref="http://www.contrib.andrew.cmu.edu/~shadow/sql/sql1992.txt"/>
	/// <see cref="http://savage.net.au/SQL/sql-2003-2.bnf.html"/>
	class SQLQueryManager
	{

		[Flags]
		private enum Specific
		{
			None = 0x0,
			OracleNullExtension = 0x1,
		}

		[Flags]
		private enum ColumnSpecific
		{
			None = 0x0,
			PrimaryKey = 0x1,
		}

		private readonly Func<string, string> nameEscaper;
	
		public SQLQueryManager(Func<string, string> nameEscaper)
		{
			this.nameEscaper = nameEscaper;
		}
	
		/// <summary>
		/// SQL-92, page 262<br/>
		/// Note that setting oracleNullExtension to true will make the query non-compliant
		/// </summary>
		private string _ColumnDefinition(ColumnDescription description, ColumnSpecific columnSpecific, Specific specific)
		{
			return string.Format(
				"{0} {1} {2} {3} {4}",
				nameEscaper(description.column.columnName), //column name
				description.options.type, //data type | domain name
				(description.options.defaultValue != null) ? "DEFAULT " + description.options.defaultValue : "", //NOTE possible SQL injection here! //default clause
				String.Join(
					" ",
					Enumerable.Empty<string>()
					.ConcatScalar(description.options.isNotNull ? "NOT NULL" : ((specific & Specific.OracleNullExtension) == Specific.OracleNullExtension ? "NULL" : null))
					.ConcatScalar((columnSpecific & ColumnSpecific.PrimaryKey) == ColumnSpecific.PrimaryKey ? "PRIMARY KEY" : null)
					.ToArray()
				), //column constraint definition
				"" //collate clause
			);
		}
		
		/// <summary>
		/// SQL-92, page 283
		/// </summary>
		private string _AlterTableStatement(string table, string alterTableAction)
		{
			return string.Format(
				"alter table {0} {1}",
				nameEscaper(table),
				alterTableAction
			);
		}
	
		/// <summary>
		/// SQL-92, page 284
		/// </summary>
		private string _AddColumnDefinition(ColumnDescription description)
		{
			return string.Format(
				"add {0}",
				_ColumnDefinition(description, ColumnSpecific.None, Specific.None)
			);
		}
		
		/// <summary>
		/// SQL-92, page 289
		/// </summary>
		private string _DropColumnDefinition(ColumnReference column)
		{
			return string.Format(
				"drop column {0} {1}",
				nameEscaper(column.columnName), //column name
				"" //drop behavior
			);
		}
		
		/// <summary>
		/// SQL-92, page 274
		/// </summary>
		private static string _ReferentialAction(ForeignKeyConstraint.ReferentialAction action)
		{
			switch(action)
			{
				case ForeignKeyConstraint.ReferentialAction.NoAction:
					return "NO ACTION";
				case ForeignKeyConstraint.ReferentialAction.Cascade:
					return "CASCADE";
				case ForeignKeyConstraint.ReferentialAction.SetNull:
					return "SET NULL";
				case ForeignKeyConstraint.ReferentialAction.SetDefault:
					return "SET DEFAULT";
				default:
					throw new ApplicationException(string.Format("Unknown action {0}", action));
			}
		}
		
		/// <summary>
		/// SQL-92, page 274<br/>
		/// Note that item 8 on page 276 specifies that if ON UPDATE clause is omitted, DB should act as if "NO ACTION" is implicitly stated<br/>
		/// Also, Oracle violates SQL-92 by not supporting explicit ON UPDATE clauses
		/// </summary>
		private string _UpdateRule(ForeignKeyConstraint.ReferentialAction onUpdate)
		{
			if(onUpdate == ForeignKeyConstraint.ReferentialAction.NoAction)
			{
				return "";
			}
			return string.Format(
				"ON UPDATE {0}",
				_ReferentialAction(onUpdate)
			);
		}
		
		/// <summary>
		/// SQL-92, page 274<br/>
		/// Note that item 9 on page 276 specifies that if ON DELETE clause is omitted, DB should act as if "NO ACTION" is implicitly stated<br/>
		/// Also, Oracle violates SQL-92 by not supporting explicit ON DELETE NO ACTION clause
		/// </summary>
		private string _DeleteRule(ForeignKeyConstraint.ReferentialAction onDelete)
		{
			if(onDelete == ForeignKeyConstraint.ReferentialAction.NoAction)
			{
				return "";
			}
			return string.Format(
				"ON DELETE {0}",
				_ReferentialAction(onDelete)
			);
		}
		
		/// <summary>
		/// SQL-92, page 274
		/// </summary>
		private string _ReferentialTriggeredAction(ForeignKeyConstraint constraint)
		{
			return string.Format(
				"{0} {1}",
				_UpdateRule(constraint.onUpdate),
				_DeleteRule(constraint.onDelete)
			);
		}
		
		/// <summary>
		/// SQL-92, page 274
		/// </summary>
		private string _ReferentialConstraintDefinition(ForeignKeyConstraint constraint)
		{
			return string.Format(
				"foreign key({0}) references {1} {2}",
				nameEscaper(constraint.column),
				nameEscaper(constraint.referencedTable),
				_ReferentialTriggeredAction(constraint)
			);
		}

		private string _UniqueConstraintDefinition(UniqueConstraint constraint)
		{
			return string.Format(
				"UNIQUE({0})",
				string.Join(", ", (from column in constraint.columns select nameEscaper(column)).ToArray())
			);
		}

		private string _CheckConstraintDefinition(CheckConstraint constraint)
		{
			return string.Format(
				"CHECK({0})",
				constraint.condition
			);
		}
		
		/// <summary>
		/// SQL-92, page 252
		/// </summary>
		private string _ConstraintNameDefinition(AbstractConstraint constraint)
		{
			return string.Format(
				"CONSTRAINT {0}",
				nameEscaper(constraint.name)
			);
		}
		
		/// <summary>
		/// SQL-92, page 270
		/// </summary>
		private string _TableConstraint(AbstractConstraint constraint)
		{
			return constraint.Accept<string>(_ReferentialConstraintDefinition, _UniqueConstraintDefinition, _CheckConstraintDefinition);
		}
		
		/// <summary>
		/// SQL-92, page 270
		/// </summary>
		private string _TableConstraintDefinition(AbstractConstraint constraint)
		{
			return string.Format(
				"{0} {1}",
				_ConstraintNameDefinition(constraint),
				_TableConstraint(constraint)
			);
		}
		
		/// <summary>
		/// SQL-92, page 291
		/// </summary>
		private string _AddTableConstraintDefinition(AbstractConstraint constraint)
		{
			return string.Format(
				"ADD {0}",
				_TableConstraintDefinition(constraint)
			);
		}
		
		/// <summary>
		/// SQL-92, page 292
		/// </summary>
		private string _DropTableConstraintDefinition(AbstractConstraint constraint)
		{
			return string.Format(
				"DROP CONSTRAINT {0}",
				nameEscaper(constraint.name)
			);
		}
		
		/// <summary>
		/// It seems that this feature is not mentioned in any SQL standart and is only implemented in Oracle and MySQL.
		/// However, it is similar to other SQL language structures, so it should be implemented in SQLQueryManager, not OracleDBTraits
		/// </summary>
		private string _ModifyColumnDefinitionOracleStyle(ColumnDescription description)
		{
			return string.Format(
				"modify ({0})",
				_ColumnDefinition(description, ColumnSpecific.None, Specific.OracleNullExtension)
			);
		}

		private string _TableElementList(TableDescription table) {
			return string.Format(
				"({0})",
				string.Join(
					", ",
					Enumerable.Empty<string>()
					.ConcatScalar(_ColumnDefinition(table.primaryKey, ColumnSpecific.PrimaryKey, Specific.None))
					.Concat(from column in table.columns select _ColumnDefinition(column, ColumnSpecific.None, Specific.None))
					.ToArray()
				)
			);
		}

		private string _TableDefinition(TableDescription table) {
			return string.Format(
				"CREATE TABLE {0} {1}",
				nameEscaper(table.table),
				_TableElementList(table)
			);
		}

		private string _DropTableStatement(string table)
		{
			return string.Format(
				"DROP TABLE {0} {1}",
				nameEscaper(table), //table name
				"" //drop behaviour
			);
		}
		
		public string CreateColumn(ColumnDescription description)
		{
			return _AlterTableStatement(description.column.tableName, _AddColumnDefinition(description));
		}
		
		public string RemoveColumn(ColumnReference column)
		{
			return _AlterTableStatement(column.tableName, _DropColumnDefinition(column));
		}
		
		public string ModifyColumnOracleStyle(ColumnDescription description)
		{
			return _AlterTableStatement(description.column.tableName, _ModifyColumnDefinitionOracleStyle(description));
		}
		
		public string CreateConstraint(AbstractConstraint constraint)
		{
			return _AlterTableStatement(constraint.table, _AddTableConstraintDefinition(constraint));
		}

		public string DropConstraint(AbstractConstraint constraint)
		{
			return _AlterTableStatement(constraint.table, _DropTableConstraintDefinition(constraint));
		}

		public string CreateTable(TableDescription table) {
			return _TableDefinition(table);
		}

		public string DropTable(string table) {
			return _DropTableStatement(table);
		}
	
	}
}
