using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class ChangeStoredProcedureCommand : AbstractStoredProcedureCommand
	{

		private readonly StoredProcedureBody body;
	
		private ChangeStoredProcedureCommand(int num, XElement inner) : base(num, inner)
		{
			this.body = new StoredProcedureBody(
				inner.Element("declarations") != null ? inner.Element("declarations").Value : "",
				inner.Element("body").Value
			);
		}
		
		public static ChangeStoredProcedureCommand CreateSpecific(int num, XElement inner)
		{
			return new ChangeStoredProcedureCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{
		
			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure change is not implemented yet");
			}
		
			StoredProcedureBody oldBody = transaction.GetStoredProcedureBody(this.procedure);
			/*Console.WriteLine();
			Console.WriteLine("===OLD DECLARATIONS===");
			Console.WriteLine(oldBody.declarations);
			Console.WriteLine("===OLD BODY===");
			Console.WriteLine(oldBody.body);
			Console.WriteLine("===END===");*/
			transaction.ReplaceStoredProcedureBody(this.procedure, this.body);
			return new[]
			       {
			       	new XElement("oldDeclarations", oldBody.declarations),
			       	new XElement("oldBody", oldBody.body),
			       };
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.ReplaceStoredProcedureBody(
				this.procedure,
				new StoredProcedureBody(
					commandRollbackInfo.Element("oldDeclarations").Value,
					commandRollbackInfo.Element("oldBody").Value
				)
			);
		}
	}
}
