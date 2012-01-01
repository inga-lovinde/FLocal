using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class ModifyColumnCommand : AbstractColumnCommandWithOptions
	{

		private ModifyColumnCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static ModifyColumnCommand CreateSpecific(int num, XElement inner)
		{
			return new ModifyColumnCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}

			var oldOptions = transaction.GetColumnOptions(column);
			transaction.ModifyColumn(this.description);
			return new[]
			       {
			       	new XElement("type", oldOptions.type),
			       	oldOptions.defaultValue != null ? new XElement("defaultValue", oldOptions.defaultValue) : null,
			       	oldOptions.isNotNull ? new XElement("isNotNull") : null
			       };
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.ModifyColumn(
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
