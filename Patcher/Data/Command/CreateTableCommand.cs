using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class CreateTableCommand : AbstractPersistentCommand
	{

		protected readonly TableDescription table;
	
		protected CreateTableCommand(int num, XElement inner) : base(num)
		{
			this.table = XMLParser.ParseTableDescription(inner);
		}
		
		public static CreateTableCommand CreateSpecific(int num, XElement inner)
		{
			return new CreateTableCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}

			transaction.CreateTable(this.table);

			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.RemoveTable(this.table);
		}
	}
}
