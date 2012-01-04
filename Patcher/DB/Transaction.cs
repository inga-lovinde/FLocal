using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class Transaction : IDisposable
	{
	
		private DbConnection connection;
		private DbTransaction transaction;
		private readonly IDBTraits traits;
		private bool isClosed;
		
		public Transaction(IDBTraits traits, string connectionString)
		{
			this.traits = traits;
			DbConnection _connection = null;
			DbTransaction _transaction = null;
			try
			{
				_connection = traits.CreateConnection(connectionString);
				_transaction = _connection.BeginTransaction(IsolationLevel.Serializable);
				this.connection = _connection;
				this.transaction = _transaction;
			} catch(Exception)
			{
				if(_transaction != null)
				{
					_transaction.Dispose();
					this.transaction = null;
				}
				if(_connection != null)
				{
					_connection.Dispose();
					this.connection = null;
				}
				throw;
			}
		}
		
		private void CheckOpened()
		{
			if(this.isClosed) throw new ApplicationException("Connection is already closed");
		}
		
		private DbCommand CreateTextCommand()
		{
			this.CheckOpened();
			DbCommand command = this.connection.CreateCommand();
			try
			{
				command.Transaction = this.transaction;
				command.CommandType = CommandType.Text;
				return command;
			} catch(Exception)
			{
				command.Dispose();
				throw;
			}
		}
		
		public string MarkParam(string paramName)
		{
			return this.traits.MarkParam(paramName);
		}
		
		public string EscapeName(string name)
		{
			return this.traits.EscapeName(name);
		}

		public void Commit()
		{
			this.CheckOpened();
			this.isClosed = true;
			this.transaction.Commit();
			this.transaction = null;
			this.connection.Close();
			this.connection = null;
		}
	
		public void Dispose()
		{
			this.isClosed = true;
			if(this.transaction != null)
			{
				try
				{
					this.transaction.Rollback();
					this.transaction.Dispose();
				} catch(SystemException)
				{
					throw;
				} catch(Exception e)
				{
					Console.WriteLine(e.ToString());
				}
				this.transaction = null;
			}
			if(this.connection != null)
			{
				try
				{
					if(this.connection.State == ConnectionState.Open)
					{
						this.connection.Close();
					}
					this.connection.Dispose();
				} catch(SystemException)
				{
					throw;
				} catch(Exception e) //Dispose should not throw exceptions
				{
					Console.WriteLine(e.ToString());
				}
				this.connection = null;
			}
		}
		
		private static DbType DetectParameterType(object value)
		{
			if(value is string)
			{
				return DbType.String;
			}
			if((value is decimal) || (value is int) || (value is long) || (value is short))
			{
				return DbType.Decimal;
			}
			if((value is float) || (value is double))
			{
				return DbType.Double;
			}
			if(value is DateTime)
			{
				return DbType.DateTime;
			}
			return DbType.Object;
		}
		
		private DbCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
		{
			DbCommand command = this.CreateTextCommand();
			command.CommandText = commandText;
			foreach(var kvp in parameters)
			{
				DbParameter parameter = command.CreateParameter();
				parameter.DbType = DetectParameterType(kvp.Value);
				parameter.ParameterName = kvp.Key;
				parameter.Value = kvp.Value;
				command.Parameters.Add(parameter);
			}
			return command;
		}
		
		public IEnumerable<Dictionary<string, string>> ExecuteReader(string commandText, Dictionary<string, object> parameters)
		{
			using(DbCommand command = this.CreateCommand(commandText, parameters))
			{
				Logger.instance.Log(commandText);
				using(DbDataReader reader = command.ExecuteReader())
				{
					//Console.WriteLine(String.Join(";", (from i in Enumerable.Range(0, reader.FieldCount) select i + ":" + reader.GetName(i)).ToArray()));
					while(reader.Read())
					{
						yield return (from i in Enumerable.Range(0, reader.FieldCount) select new KeyValuePair<string, string>(reader.GetName(i), reader.GetValue(i).ToString())).ToDictionary();
					}
				}
			}
		}
		
		public IEnumerable<Dictionary<string, string>> ExecuteReader(string commandText)
		{
			return this.ExecuteReader(commandText, new Dictionary<string, object>());
		}

		public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters)
		{
			using(DbCommand command = this.CreateCommand(commandText, parameters))
			{
				Logger.instance.Log(commandText);
				return command.ExecuteNonQuery();
			}
		}
		
		public int ExecuteNonQuery(string commandText)
		{
			return this.ExecuteNonQuery(commandText, new Dictionary<string, object>());
		}
		
		//SELECT LINE, TEXT FROM ALL_SOURCE WHERE TYPE = 'PACKAGE BODY' AND NAME = '' AND OWNER = (SELECT USER FROM DUAL) for update
		
		public StoredProcedureBody GetStoredProcedureBody(StoredProcedureReference procedure)
		{
			return this.traits.GetStoredProcedureBody(this.CreateTextCommand, procedure);
		}
		
		public void ReplaceStoredProcedureBody(StoredProcedureReference procedure, StoredProcedureBody newBody)
		{
			this.traits.ReplaceStoredProcedureBody(this.CreateTextCommand, procedure, newBody);
		}
		
		public void RemoveStoredProcedure(StoredProcedureReference procedure)
		{
			this.traits.RemoveStoredProcedure(this.CreateTextCommand, procedure);
		}
		
		public void CreateStoredProcedure(StoredProcedureReference procedure, StoredProcedureBody body)
		{
			this.traits.CreateStoredProcedure(this.CreateTextCommand, procedure, body);
		}
		
		public ColumnOptions GetColumnOptions(ColumnReference column)
		{
			return this.traits.GetColumnOptions(this.CreateTextCommand, column);
		}
		
		public void RemoveColumn(ColumnReference column)
		{
			this.traits.RemoveColumn(this.CreateTextCommand, column);
		}
		
		public void CreateColumn(ColumnDescription description)
		{
			this.traits.CreateColumn(this.CreateTextCommand, description);
		}

		public void ModifyColumn(ColumnDescription description)
		{
			this.traits.ModifyColumn(this.CreateTextCommand, description);
		}
		
		public void CreateConstraint(AbstractConstraint constraint)
		{
			this.traits.CreateConstraint(this.CreateTextCommand, constraint);
		}
		
		public void RemoveConstraint(AbstractConstraint constraint)
		{
			this.traits.RemoveConstraint(this.CreateTextCommand, constraint);
		}

		public void CreateTable(TableDescription table)
		{
			this.traits.CreateTable(this.CreateTextCommand, table);
		}

		public void RemoveTable(TableDescription table)
		{
			this.traits.RemoveTable(this.CreateTextCommand, table);
		}

		public string GetViewBody(string viewName)
		{
			return this.traits.GetViewBody(this.CreateTextCommand, viewName);
		}

		public void CreateView(string viewName, string body)
		{
			this.traits.CreateView(this.CreateTextCommand, viewName, body);
		}

		public void RemoveView(string viewName)
		{
			this.traits.RemoveView(this.CreateTextCommand, viewName);
		}

	}
	
}
