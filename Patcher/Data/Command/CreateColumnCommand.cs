using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class CreateColumnCommand : AbstractColumnCommandWithOptions
	{

		private CreateColumnCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static CreateColumnCommand CreateSpecific(int num, XElement inner)
		{
			return new CreateColumnCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}
		
			transaction.CreateColumn(this.description);
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.RemoveColumn(this.column);
		}
	}
}
