using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
//using Devart.Data.Oracle;

namespace Patcher.DB
{

	class OracleDBTraits : IDBTraits
	{
	
		public static readonly IDBTraits instance = new OracleDBTraits();
		
		protected OracleDBTraits()
		{
		}
	
		private static readonly Regex ALPHANUMERIC = new Regex("^[a-zA-Z]\\w*$", RegexOptions.Compiled | RegexOptions.Singleline);
		
		private static string _EscapeName(string name)
		{
			if(!ALPHANUMERIC.IsMatch(name)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return string.Format("\"{0}\"", name);
		}
	
		string IDBTraits.EscapeName(string name)
		{
			return _EscapeName(name);
		}

		DbConnection IDBTraits.CreateConnection(string connectionString)
		{
#pragma warning disable 0618
			OracleConnection connection = new OracleConnection(connectionString);
#pragma warning restore 0618
			connection.Open();
			return connection;
		}
		
		string IDBTraits.MarkParam(string paramName)
		{
			if(!ALPHANUMERIC.IsMatch(paramName)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return ":" + paramName;
		}
		
		string IDBTraits.ParamName(string paramName)
		{
			if(!ALPHANUMERIC.IsMatch(paramName)) throw new ApplicationException("Name should contain only alphanumeric characters");
			return paramName;
		}
		
		private static void AddParam(DbCommand command, string name, DbType type, object value)
		{
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.DbType = type;
			param.Value = value;
			command.Parameters.Add(param);
		}
		
		private static bool ParseBoolString(string value)
		{
			switch(value.ToLower())
			{
				case "n":
					return false;
				case "y":
					return true;
				default:
					throw new ApplicationException(string.Format("Unknown value {0}", value));
			}
		}
		
		private static T CastResult<T>(object value) where T : class
		{
			if(DBNull.Value.Equals(value))
			{
				return null;
			} else
			{
				return (T)value;
			}
		}
		
		private static T? CastScalarResult<T>(object value) where T : struct
		{
			if(DBNull.Value.Equals(value))
			{
				return null;
			} else
			{
				return (T)value;
			}
		}
		
		private static readonly SQLQueryManager _SQLQueryManager = new SQLQueryManager(_EscapeName);
		
		private static string[] GetPackage(Func<DbCommand> commandCreator, string package, string type)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT LINE, TEXT FROM USER_SOURCE WHERE TYPE = :ptype AND NAME = :pname order by line";
				AddParam(command, "ptype", DbType.String, type);
				AddParam(command, "pname", DbType.String, package);
				List<string> lines = new List<string>();
				using(DbDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						lines.Add(reader.GetValue(reader.GetOrdinal("TEXT")).ToString());
					}
				}
				return lines.ToArray();
			}
		}

		private static readonly Regex PROCEDURE_BODY_HEADER_REGEX = new Regex(
			"^procedure" +
			"\\s+" +
			"(?<name>\\w+)" +
			"\\s*" +
			"\\(" +
				"(?<params>" +
					"(\\w+\\s+(in|out)\\s+\\w+)" +
					"(\\,\\s*\\w+\\s+(in|out)\\s+\\w+)*" +
				")?" +
			"\\)$",
			RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled
		);
		private static readonly Regex PROCEDURE_BODY_FOOTER_REGEX = new Regex(
			"^end" +
			"\\s+" +
			"(?<name>\\w+)" +
			";$",
			RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled
		);

		private static readonly Dictionary<StoredProcedureReference.ParameterDirection, string> DIRECTIONS = new Dictionary<StoredProcedureReference.ParameterDirection, string>
		                                                                                                       {
		                                                                                                       	{ StoredProcedureReference.ParameterDirection.In, "in" },
		                                                                                                       	{ StoredProcedureReference.ParameterDirection.Out, "out" },
		                                                                                                       };
		
		private struct ProcedureBodyPosition
		{
			public int header;
			public int begin;
			public int end;
		}
		
		private class ProcedureNotFoundException : FormattableException
		{
			public ProcedureNotFoundException(string message, params object[] args) : base(message, args)
			{
			}
		}
		
		private class ViewNotFoundException : FormattableException
		{
			public ViewNotFoundException(string message, params object[] args) : base(message, args)
			{
			}
		}
		
		private static int FindProcedureHeader(string[] package, StoredProcedureReference procedure)
		{
			for(int i=0; i<package.Length; i++)
			{
				var line = package[i].Trim().TrimEnd(';');
				if (PROCEDURE_BODY_HEADER_REGEX.IsMatch(line))
				{
					var match = PROCEDURE_BODY_HEADER_REGEX.Match(line);
					string name = match.Groups["name"].Value;
					string[] parameters = match.Groups["params"].Value.Split(',');
					if(name == procedure.procedureName)
					{
						if(parameters.Length != procedure.parameters.Length)
						{
							throw new ApplicationException(String.Format("Parameters number mismatch for procedure {0}: expected {1}, got {2}", procedure.procedureName, procedure.parameters.Length, parameters.Length));
						}
						for(int j=0; j<parameters.Length; j++)
						{
							string[] parts = parameters[j].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
							if(parts[0] != procedure.parameters[j].name)
							{
								throw new ApplicationException(String.Format("Parameter {1} name mismatch for procedure {0}: expected {2}, got {3}", j, procedure.procedureName, procedure.parameters[j].name, parts[0]));
							}
							if(parts[1].ToLower() != DIRECTIONS[procedure.parameters[j].direction])
							{
								throw new ApplicationException(String.Format("Parameter {1} direction mismatch for procedure {0}: expected {2}, got {3}", procedure.procedureName, procedure.parameters[j].name, DIRECTIONS[procedure.parameters[j].direction], parts[1]));
							}
							if(parts[2] != procedure.parameters[j].type)
							{
								throw new ApplicationException(String.Format("Parameter {1} type mismatch for procedure {0}: expected {2}, got {3}", j, procedure.procedureName, procedure.parameters[j].type, parts[2]));
							}
						}
						return i;
					}
				}
			}
			throw new ProcedureNotFoundException(String.Format("Procedure {0} header not found", procedure.procedureName));
		}
		
		private static ProcedureBodyPosition FindProcedureBodyPosition(string[] package, StoredProcedureReference procedure)
		{
			int header = FindProcedureHeader(package, procedure);
			int? begin = null;
			int? end = null;
			if(package.Length <= header+2)
			{
				throw new ApplicationException("Unexpected package end");
			}
			if(package[header+1].Trim().ToLower() != "is")
			{
				throw new ApplicationException(String.Format("Incorrect line follows procedure {0} header: '{1}'", procedure.procedureName, package[header+1].Trim()));
			}
			for(int i=header+2; i<package.Length; i++)
			{
				if(package[i].Trim().ToLower() == "begin")
				{
					begin = i;
					break;
				}
			}
			if(!begin.HasValue)
			{
				throw new ApplicationException(String.Format("Unable to find body begin for procedure {0}", procedure.procedureName));
			}
			if(package.Length <= begin+1)
			{
				throw new ApplicationException("Unexpected package end");
			}
			for(int i=begin.Value+1; i<package.Length; i++)
			{
				string line = package[i].Trim();
				if(PROCEDURE_BODY_FOOTER_REGEX.IsMatch(line))
				{
					var match = PROCEDURE_BODY_FOOTER_REGEX.Match(line);
					var name = match.Groups["name"].Value;
					if(name != procedure.procedureName)
					{
						throw new ApplicationException(String.Format("Wrong procedure footer: expected {0}, got {1}", procedure.procedureName, name));
					}
					end = i;
					break;
				}
			}
			if(!end.HasValue)
			{
				throw new ApplicationException("Procedure footer not found");
			}
			return new ProcedureBodyPosition
			       {
			       	header = header,
			       	begin = begin.Value,
			       	end = end.Value,
			       };
		}

		StoredProcedureBody IDBTraits.GetStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure)
		{
			string[] package = GetPackage(commandCreator, procedure.packageName, "PACKAGE BODY");
			var position = FindProcedureBodyPosition(package, procedure);
			StringBuilder declarationsBuilder = new StringBuilder();
			for(int i=position.header+2; i<position.begin; i++)
			{
				declarationsBuilder.Append(package[i]);
			}
			StringBuilder bodyBuilder = new StringBuilder();
			for(int i=position.begin+1; i<position.end; i++)
			{
				bodyBuilder.AppendFormat(package[i]);
			}
			return new StoredProcedureBody(declarationsBuilder.ToString(), bodyBuilder.ToString());
		}
		
		private static string FormatStoredProcedureHeader(StoredProcedureReference procedure)
		{
			return string.Format("procedure {0}({1})", procedure.procedureName, String.Join(", ", (from param in procedure.parameters select String.Format("{0} {1} {2}", param.name, DIRECTIONS[param.direction], param.type)).ToArray()));
		}
		
		private static string FormatStoredProcedure(StoredProcedureReference procedure, StoredProcedureBody body)
		{
			StringBuilder result = new StringBuilder();
			result.Append(FormatStoredProcedureHeader(procedure));
			result.AppendLine();
			result.AppendLine("is");
			if(body.declarations.Trim() != "")
			{
				result.AppendLine(body.declarations.TrimEnd().Trim('\r', '\n'));
			}
			result.AppendLine("begin");
			if(body.body.Trim() != "")
			{
				result.AppendLine(body.body.TrimEnd().Trim('\r', '\n'));
			}
			result.AppendFormat("end {0};", procedure.procedureName);
			result.AppendLine();
			return result.ToString();
		}
		
		void IDBTraits.ReplaceStoredProcedureBody(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody newBody)
		{
			string[] package = GetPackage(commandCreator, procedure.packageName, "PACKAGE BODY");
			var position = FindProcedureBodyPosition(package, procedure);
			StringBuilder packageBuilder = new StringBuilder();
			packageBuilder.AppendLine("create or replace");
			for(int i=0; i<position.header; i++)
			{
				packageBuilder.Append(package[i]);
			}
			packageBuilder.Append(FormatStoredProcedure(procedure, newBody));
			for(int i=position.end+1; i<package.Length; i++)
			{
				packageBuilder.AppendFormat(package[i]);
			}
			/*Console.WriteLine();
			Console.WriteLine("===NEW PACKAGE===");
			Console.WriteLine(packageBuilder.ToString());
			Console.WriteLine("===END PACKAGE===");*/
			using(DbCommand command = commandCreator())
			{
				command.CommandText = packageBuilder.ToString();
				command.ExecuteNonQuery();
			}
		}

		void IDBTraits.RemoveStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure)
		{
			{
				string[] packageSpec = GetPackage(commandCreator, procedure.packageName, "PACKAGE");
				int position = FindProcedureHeader(packageSpec, procedure);
				StringBuilder packageSpecBuilder = new StringBuilder();
				packageSpecBuilder.AppendLine("create or replace");
				for(int i=0; i<position; i++)
				{
					packageSpecBuilder.Append(packageSpec[i]);
				}
				for(int i=position+1; i<packageSpec.Length; i++)
				{
					packageSpecBuilder.Append(packageSpec[i]);
				}
				using(DbCommand command = commandCreator())
				{
					command.CommandText = packageSpecBuilder.ToString();
					/*Console.WriteLine();
					Console.WriteLine("=====PACKAGE SPEC=====");
					Console.WriteLine(command.CommandText);
					Console.WriteLine("=====END PACKAGE SPEC=====");*/
					command.ExecuteNonQuery();
				}
			}
			{
				string[] package = GetPackage(commandCreator, procedure.packageName, "PACKAGE BODY");
				var position = FindProcedureBodyPosition(package, procedure);
				StringBuilder packageBuilder = new StringBuilder();
				packageBuilder.AppendLine("create or replace");
				for(int i=0; i<position.header; i++)
				{
					packageBuilder.Append(package[i]);
				}
				for(int i=position.end+1; i<package.Length; i++)
				{
					packageBuilder.Append(package[i]);
				}
				using(DbCommand command = commandCreator())
				{
					command.CommandText = packageBuilder.ToString();
					/*Console.WriteLine();
					Console.WriteLine("=====PACKAGE BODY=====");
					Console.WriteLine(command.CommandText);
					Console.WriteLine("=====END PACKAGE BODY=====");*/
					command.ExecuteNonQuery();
				}
			}
		}

		void IDBTraits.CreateView(Func<DbCommand> commandCreator, string viewName, string body)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = string.Format("CREATE VIEW {0} as {1}", _EscapeName(viewName), body);
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		void IDBTraits.RemoveView(Func<DbCommand> commandCreator, string viewName)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = string.Format("DROP VIEW {0}", _EscapeName(viewName));
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		void IDBTraits.CreateStoredProcedure(Func<DbCommand> commandCreator, StoredProcedureReference procedure, StoredProcedureBody body)
		{
			string packageEnd = string.Format("end {0};", procedure.packageName);
			string packageEndMarker = packageEnd.ToLower();
			{
				string[] package = GetPackage(commandCreator, procedure.packageName, "PACKAGE BODY");
				try
				{
					var position = FindProcedureBodyPosition(package, procedure);
					throw new ApplicationException(String.Format("Procedure {0} body is already declared", procedure.procedureName));
				} catch(ProcedureNotFoundException)
				{
				}
				StringBuilder packageBuilder = new StringBuilder();
				packageBuilder.AppendLine("create or replace");
				for(int i=0; i<package.Length; i++)
				{
					if(package[i].Trim().ToLower() == packageEndMarker)
					{
						if(package.Length > i+1)
						{
							throw new ApplicationException("Unexpected package end");
						}
						break;
					}
					packageBuilder.Append(package[i]);
				}
				packageBuilder.Append(FormatStoredProcedure(procedure, body));
				packageBuilder.AppendLine(packageEnd);
				using(DbCommand command = commandCreator())
				{
					command.CommandText = packageBuilder.ToString();
					/*Console.WriteLine();
					Console.WriteLine("=====PACKAGE BODY=====");
					Console.WriteLine(command.CommandText);
					Console.WriteLine("=====END PACKAGE BODY=====");*/
					command.ExecuteNonQuery();
				}
			}
			{
				string[] packageSpec = GetPackage(commandCreator, procedure.packageName, "PACKAGE");
				try
				{
					var position = FindProcedureHeader(packageSpec, procedure);
					throw new ApplicationException(String.Format("Procedure {0} is already declared", procedure.procedureName));
				} catch(ProcedureNotFoundException)
				{
				}
				StringBuilder packageSpecBuilder = new StringBuilder();
				packageSpecBuilder.AppendLine("create or replace");
				for(int i=0; i<packageSpec.Length; i++)
				{
					if(packageSpec[i].Trim().ToLower() == packageEndMarker)
					{
						if(packageSpec.Length > i+1)
						{
							throw new ApplicationException("Unexpected package end");
						}
						break;
					}
					packageSpecBuilder.Append(packageSpec[i]);
				}
				packageSpecBuilder.Append(FormatStoredProcedureHeader(procedure));
				packageSpecBuilder.AppendLine(";");
				packageSpecBuilder.AppendLine(packageEnd);
				using(DbCommand command = commandCreator())
				{
					command.CommandText = packageSpecBuilder.ToString();
					/*Console.WriteLine();
					Console.WriteLine("=====PACKAGE SPEC=====");
					Console.WriteLine(command.CommandText);
					Console.WriteLine("=====END PACKAGE SPEC=====");*/
					command.ExecuteNonQuery();
				}
			}
		}
		
		string IDBTraits.GetViewBody(Func<DbCommand> commandCreator, string name)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT TEXT FROM USER_VIEWS WHERE VIEW_NAME = :pname";
				AddParam(command, "pname", DbType.String, name);
				using(DbDataReader reader = command.ExecuteReader())
				{
					if(!reader.Read())
					{
						throw new ViewNotFoundException("View {0} not found", name);
					}
					return reader.GetValue("TEXT").ToString();
				}
			}
		}
		
		ColumnOptions IDBTraits.GetColumnOptions(Func<DbCommand> commandCreator, ColumnReference column)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "select * from user_tab_columns where table_name = :ptable and column_name = :pcolumn";
				AddParam(command, "ptable", DbType.String, column.tableName);
				AddParam(command, "pcolumn", DbType.String, column.columnName);
				using(DbDataReader reader = command.ExecuteReader())
				{
					if(!reader.Read())
					{
						throw new ApplicationException("Column not found");
					}

					string dataType = (string)reader.GetValue("DATA_TYPE");

					decimal? dataLength = CastScalarResult<decimal>(reader.GetValue("DATA_LENGTH"));
					decimal? dataPrecision = CastScalarResult<decimal>(reader.GetValue("DATA_PRECISION"));
					decimal? dataScale = CastScalarResult<decimal>(reader.GetValue("DATA_SCALE"));
					string sizeSpecifier = null;
					if(dataPrecision.HasValue)
					{
						if(dataScale.HasValue)
						{
							sizeSpecifier = string.Format("{0}, {1}", dataPrecision, dataScale);
						}
						else
						{
							sizeSpecifier = dataPrecision.ToString();
						}
					}
					else if(dataLength.HasValue)
					{
						sizeSpecifier = dataLength.ToString();
					}
					else
					{
						throw new ApplicationException("Field without a length");
					}

					string defaultValue = CastResult<string>(reader.GetValue(reader.GetOrdinal("DATA_DEFAULT")));

					return new ColumnOptions(
						string.Format("{0}({1})", dataType, sizeSpecifier),
						defaultValue != null ? defaultValue.Trim() : null,
						!ParseBoolString((string)reader.GetValue(reader.GetOrdinal("NULLABLE")))
					);
				}
			}
		}
		
		void IDBTraits.RemoveColumn(Func<DbCommand> commandCreator, ColumnReference column)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.RemoveColumn(column);
				command.ExecuteNonQuery();
			}
		}
		
		void IDBTraits.CreateColumn(Func<DbCommand> commandCreator, ColumnDescription description)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.CreateColumn(description);
				command.ExecuteNonQuery();
			}
		}
		
		void IDBTraits.ModifyColumn(Func<DbCommand> commandCreator, ColumnDescription description)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.ModifyColumnOracleStyle(description);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}
		
		private static string GetStringRepresentation(ForeignKeyConstraint.ReferentialAction action)
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
					throw new ApplicationException("Unknown referential action");
			}
		}
		
		private void CheckConstraint(Func<DbCommand> commandCreator, ForeignKeyConstraint constraint)
		{

			string referenced;
		
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONSTRAINTS WHERE TABLE_NAME = :ptable and CONSTRAINT_NAME = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				using(var reader = command.ExecuteReader())
				{
					if(!reader.Read())
					{
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}
					
					if(reader.GetValue("CONSTRAINT_TYPE").ToString() != "R")
					{
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "R", reader.GetValue("CONSTRAINT_TYPE"));
					}

					if(reader.GetValue("DELETE_RULE").ToString() != GetStringRepresentation(constraint.onDelete))
					{
						throw new FormattableException("Constraint {0}.{1} wrong on delete action: expected {2}, got {3}", constraint.table, constraint.name, GetStringRepresentation(constraint.onDelete), reader.GetValue("DELETE_RULE"));
					}

					referenced = reader.GetValue("R_CONSTRAINT_NAME").ToString();
				}
			}
			
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONSTRAINTS WHERE CONSTRAINT_NAME = :pconstraint AND CONSTRAINT_TYPE = :ptype";
				AddParam(command, "pconstraint", DbType.String, referenced);
				AddParam(command, "ptype", DbType.String, "P");
				
				using(var reader = command.ExecuteReader())
				{
					reader.Read();
					if(reader.GetValue("TABLE_NAME").ToString() != constraint.referencedTable)
					{
						throw new FormattableException("Constraint {0}.{1} wrong referenced table: expected {2}, got {3}", constraint.table, constraint.name, constraint.referencedTable, reader.GetValue("TABLE_NAME"));
					}
				}
			}
			
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONS_COLUMNS WHERE TABLE_NAME = :ptable AND CONSTRAINT_NAME = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				
				using(var reader = command.ExecuteReader())
				{
					reader.Read();
					if(reader.GetValue("COLUMN_NAME").ToString() != constraint.column)
					{
						throw new FormattableException("Constraint {0}.{1} wrong column: expected {2}, got {3}", constraint.table, constraint.name, constraint.column, reader.GetValue("COLUMN_NAME"));
					}
				}
			}
		}
		
		private void CheckConstraint(Func<DbCommand> commandCreator, UniqueConstraint constraint)
		{

			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONSTRAINTS WHERE TABLE_NAME = :ptable and CONSTRAINT_NAME = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				using(var reader = command.ExecuteReader())
				{
					if(!reader.Read())
					{
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}
					
					if(reader.GetValue("CONSTRAINT_TYPE").ToString() != "U")
					{
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "U", reader.GetValue("CONSTRAINT_TYPE"));
					}
				}
			}
			
			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONS_COLUMNS WHERE TABLE_NAME = :ptable AND CONSTRAINT_NAME = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				
				using(var reader = command.ExecuteReader())
				{
					HashSet<string> dbColumns = new HashSet<string>();
					while(reader.Read())
					{
						dbColumns.Add(reader.GetValue("COLUMN_NAME").ToString());
					}
					
					if(!dbColumns.IsSubsetOf(constraint.columns))
					{
						throw new FormattableException("Some columns are not mentioned in constraint definition: {0}", string.Join(",", dbColumns.Except(constraint.columns).ToArray()));
					}
					if(!dbColumns.IsSupersetOf(constraint.columns))
					{
						throw new FormattableException("Some columns are missed in DB: {0}", string.Join(",", constraint.columns.Except(dbColumns).ToArray()));
					}
				}
			}
		}
		
		private void CheckConstraint(Func<DbCommand> commandCreator, CheckConstraint constraint)
		{

			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM USER_CONSTRAINTS WHERE TABLE_NAME = :ptable and CONSTRAINT_NAME = :pconstraint";
				AddParam(command, "ptable", DbType.String, constraint.table);
				AddParam(command, "pconstraint", DbType.String, constraint.name);
				using(var reader = command.ExecuteReader())
				{
					if(!reader.Read())
					{
						throw new FormattableException("Constraint {0}.{1} not found", constraint.table, constraint.name);
					}
					
					if(reader.GetValue("CONSTRAINT_TYPE").ToString() != "C")
					{
						throw new FormattableException("Constraint {0}.{1} wrong type: expected {2}, got {3}", constraint.table, constraint.name, "C", reader.GetValue("CONSTRAINT_TYPE"));
					}

					if(reader.GetValue("SEARCH_CONDITION").ToString() != constraint.condition)
					{
						throw new FormattableException("Constraint {0}.{1} wrong condition: expected {2}, got {3}", constraint.table, constraint.name, constraint.condition, reader.GetValue("SEARCH_CONDITION"));
					}
				}
			}
		}
		
		private void CheckConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint)
		{
			constraint.Accept(fkc => CheckConstraint(commandCreator, fkc), uc => CheckConstraint(commandCreator, uc), cc => CheckConstraint(commandCreator, cc));
		}

		public void RemoveConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint)
		{
			CheckConstraint(commandCreator, constraint);
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.DropConstraint(constraint);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public void CreateConstraint(Func<DbCommand> commandCreator, AbstractConstraint constraint)
		{
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.CreateConstraint(constraint);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		public void CreateTable(Func<DbCommand> commandCreator, TableDescription table) {
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.CreateTable(table);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		private void CheckTable(Func<DbCommand> commandCreator, TableDescription table) {

			HashSet<string> columns = new HashSet<string>(from column in table.columns select column.column.columnName);
			columns.Add(table.primaryKey.column.columnName);

			using(DbCommand command = commandCreator())
			{
				command.CommandText = "SELECT * FROM user_tab_columns WHERE TABLE_NAME = :ptable";
				AddParam(command, "ptable", DbType.String, table.table);
				
				using(var reader = command.ExecuteReader())
				{
					HashSet<string> dbColumns = new HashSet<string>();
					while(reader.Read())
					{
						dbColumns.Add(reader.GetValue("COLUMN_NAME").ToString());
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
			Console.WriteLine();
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.type, options.type);
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.defaultValue, options.defaultValue);
			Console.WriteLine("'{0}' vs '{1}'", table.primaryKey.options.isNotNull, options.isNotNull);
			if(!table.primaryKey.options.Equals((this as IDBTraits).GetColumnOptions(commandCreator, table.primaryKey.column))) {
				throw new FormattableException("Column {0} definition mismatch", table.primaryKey.column.columnName);
			}

			foreach(var column in table.columns) {
				options = (this as IDBTraits).GetColumnOptions(commandCreator, column.column);
				Console.WriteLine();
				Console.WriteLine("'{0}' vs '{1}'", column.options.type, options.type);
				Console.WriteLine("'{0}' vs '{1}'", column.options.defaultValue, options.defaultValue);
				Console.WriteLine("'{0}' vs '{1}'", column.options.isNotNull, options.isNotNull);
				if(!column.options.Equals((this as IDBTraits).GetColumnOptions(commandCreator, column.column))) {
					throw new FormattableException("Column {0} definition mismatch", column.column.columnName);
				}
			}
		}

		void IDBTraits.RemoveTable(Func<DbCommand> commandCreator, TableDescription table) {
			this.CheckTable(commandCreator, table);
			using(DbCommand command = commandCreator())
			{
				command.CommandText = _SQLQueryManager.DropTable(table.table);
				Console.WriteLine();
				Console.WriteLine(command.CommandText);
				command.ExecuteNonQuery();
			}
		}

		bool IDBTraits.IsDDLTransactional
		{
			get { return false; }
		}
		
	}
}
