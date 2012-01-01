using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class CreateViewCommand : AbstractViewCommand
	{

		private readonly string body;
	
		private CreateViewCommand(int num, XElement inner) : base(num, inner)
		{
			this.body = inner.Element("body").Value;
		}
		
		public static CreateViewCommand CreateSpecific(int num, XElement inner)
		{
			return new CreateViewCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}
		
			transaction.CreateView(this.viewName, this.body);
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.RemoveView(this.viewName);
		}
	}
}
