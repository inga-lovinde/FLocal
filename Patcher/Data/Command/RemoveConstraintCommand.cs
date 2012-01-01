using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class RemoveConstraintCommand : AbstractConstraintCommand
	{

		private RemoveConstraintCommand(int num, XElement inner) : base(num, inner)
		{
		}
		
		public static RemoveConstraintCommand CreateSpecific(int num, XElement inner)
		{
			return new RemoveConstraintCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity)
		{

			if(!forceIntegrity)
			{
				throw new NotImplementedException("Safe stored procedure creation is not implemented yet");
			}

			transaction.RemoveConstraint(this.constraint);
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			transaction.CreateConstraint(this.constraint);
		}
	}
}
