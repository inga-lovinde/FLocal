using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class RemoveViewCommand : AbstractViewCommand
	{

		private RemoveViewCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static RemoveViewCommand CreateSpecific(int num, XElement inner)
		{
			return new RemoveViewCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}
		
			string body = transaction.GetViewBody(this.viewName);

			transaction.RemoveView(this.viewName);
			return new XElement[] {
						 new XElement("body", body),
						};
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.CreateView(this.viewName, commandRollbackInfo.Element("body").Value);
		}
	}
}
