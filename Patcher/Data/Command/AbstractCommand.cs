using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractCommand
	{

		private static readonly Dictionary<string, Func<int, XElement, AbstractCommand>> creators = new Dictionary<string, Func<int, XElement, AbstractCommand>>
		                                                                                            {
																										{ "sql", SqlCommand.CreateSpecific },
																										{ "createProcedure", CreateStoredProcedureCommand.CreateSpecific },
																										{ "changeProcedure", ChangeStoredProcedureCommand.CreateSpecific },
																										{ "removeProcedure", RemoveStoredProcedureCommand.CreateSpecific },
																										{ "createColumn", CreateColumnCommand.CreateSpecific },
																										{ "removeColumn", RemoveColumnCommand.CreateSpecific },
																										{ "modifyColumn", ModifyColumnCommand.CreateSpecific },
																										{ "createConstraint", CreateConstraintCommand.CreateSpecific },
																										{ "removeConstraint", RemoveConstraintCommand.CreateSpecific },
																										{ "createTable", CreateTableCommand.CreateSpecific },
																										{ "createView", CreateViewCommand.CreateSpecific },
																										{ "removeView", RemoveViewCommand.CreateSpecific },
		                                                                                            };

		public readonly int num;

		protected AbstractCommand(int num)
		{
			this.num = num;
		}
		
		public static AbstractCommand Create(XElement data)
		{
			int num = data.Attribute("num") != null ? int.Parse(data.Attribute("num").Value) : 0;
			XElement inner = data.Elements().Single();
			return creators[inner.Name.ToString()](num, inner);
		}

		public abstract IEnumerable<XElement> Apply(Transaction transaction);

		public abstract void Rollback(Transaction transaction, XElement commandRollbackInfo);
		
		protected void Execute(string sql, Transaction transaction)
		{
			try
			{
				transaction.ExecuteNonQuery(sql);
			} catch(Exception e)
			{
				throw new ApplicationException("Error while executing query <" + sql + ">", e);
			}
		}
		
	}
}
