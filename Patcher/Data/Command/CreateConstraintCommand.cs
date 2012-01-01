using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class CreateConstraintCommand : AbstractConstraintCommand
	{

		private CreateConstraintCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static CreateConstraintCommand CreateSpecific(int num, XElement inner)
		{
			return new CreateConstraintCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}
		
			transaction.CreateConstraint(this.constraint);
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.RemoveConstraint(this.constraint);
		}
	}
}
