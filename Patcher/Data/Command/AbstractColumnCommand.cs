using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractColumnCommand : AbstractPersistentCommand
	{

		protected readonly ColumnReference column;
	
		protected AbstractColumnCommand(int num, XElement inner) : base(num)
		{
			this.column = XMLParser.ParseColumnReference(inner);
		}
		
	}
}
