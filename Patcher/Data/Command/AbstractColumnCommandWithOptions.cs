using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractColumnCommandWithOptions : AbstractColumnCommand
	{

		protected readonly ColumnOptions options;
		protected ColumnDescription description
		{
			get
			{
				return new ColumnDescription(this.column, this.options);
			}
		}
	
		protected AbstractColumnCommandWithOptions(int num, XElement inner) : base(num, inner)
		{
			this.options = XMLParser.ParseColumnOptions(inner);
		}
	
	}
}
