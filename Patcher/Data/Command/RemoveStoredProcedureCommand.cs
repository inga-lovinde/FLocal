using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class RemoveStoredProcedureCommand : AbstractStoredProcedureCommand
	{

		private RemoveStoredProcedureCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static RemoveStoredProcedureCommand CreateSpecific(int num, XElement inner)
		{
			return new RemoveStoredProcedureCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure removal is not implemented yet");
			}
		
			StoredProcedureBody body = transaction.GetStoredProcedureBody(this.procedure);
			transaction.RemoveStoredProcedure(this.procedure);
			return new[]
			       {
			       	new XElement("declarations", body.declarations),
			       	new XElement("body", body.body),
			       };
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.CreateStoredProcedure(
				this.procedure,
				new StoredProcedureBody(
					commandRollbackInfo.Element("declarations").Value,
					commandRollbackInfo.Element("body").Value
				)
			);
		}
	}
}
