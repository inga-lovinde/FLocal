using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class RemoveColumnCommand : AbstractColumnCommand
	{

		private RemoveColumnCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static RemoveColumnCommand CreateSpecific(int num, XElement inner)
		{
			return new RemoveColumnCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}

			var options = transaction.GetColumnOptions(column);
			/*Console.WriteLine();
			Console.Write(options.type);
			if(options.defaultValue != null)
			{
				Console.Write(" DEFAULT " + options.defaultValue);
			}
			if(options.isNotNull)
			{
				Console.Write(" NOT NULL");
			}
			Console.WriteLine();
			Console.WriteLine("'" + options.defaultValue + "'");*/
			transaction.RemoveColumn(column);
			return new[]
			       {
			       	new XElement("type", options.type),
			       	options.defaultValue != null ? new XElement("defaultValue", options.defaultValue) : null,
			       	options.isNotNull ? new XElement("isNotNull") : null
			       };
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.CreateColumn(
				new ColumnDescription(
					this.column,
					new ColumnOptions(
						commandRollbackInfo.Element("type").Value,
						commandRollbackInfo.Element("defaultValue") != null ? commandRollbackInfo.Element("defaultValue").Value : null,
						commandRollbackInfo.Element("isNotNull") != null
					)
				)
			);
		}
	}
}
