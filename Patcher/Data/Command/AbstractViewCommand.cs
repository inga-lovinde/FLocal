using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractViewCommand : AbstractPersistentCommand
	{

		protected readonly string viewName;
	
		protected AbstractViewCommand(int num, XElement inner) : base(num)
		{
			this.viewName = inner.Element("view").Value;
		}
		
	}
}
