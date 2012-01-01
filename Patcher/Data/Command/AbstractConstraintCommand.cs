using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractConstraintCommand : AbstractPersistentCommand
	{

		protected readonly AbstractConstraint constraint;
	
		protected AbstractConstraintCommand(int num, XElement inner) : base(num)
		{
			this.constraint = XMLParser.ParseConstraint(inner);
		}
		
	}
}
