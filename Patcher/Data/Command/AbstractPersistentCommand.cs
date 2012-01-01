using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractPersistentCommand : AbstractCommand
	{

		protected AbstractPersistentCommand(int num) : base(num)
		{
		}

		public sealed override IEnumerable<XElement> Apply(Transaction transaction)
		{
			throw new ApplicationException("Cannot apply persistent command");
		}

		public abstract IEnumerable<XElement> Apply(Transaction transaction, bool forceIntegrity);

	}
}
