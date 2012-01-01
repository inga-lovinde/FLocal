using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class CreateStoredProcedureCommand : AbstractStoredProcedureCommand
	{

		private readonly StoredProcedureBody body;
	
		private CreateStoredProcedureCommand(int num, XElement inner) : base(num, inner)
		{
			this.body = new StoredProcedureBody(
				inner.Element("declarations") != null ? inner.Element("declarations").Value : "",
				inner.Element("body").Value
			);
		}
		
		public static CreateStoredProcedureCommand CreateSpecific(int num, XElement inner)
		{
			return new CreateStoredProcedureCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}
		
			transaction.CreateStoredProcedure(this.procedure, this.body);
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.RemoveStoredProcedure(this.procedure);
		}
	}
}
