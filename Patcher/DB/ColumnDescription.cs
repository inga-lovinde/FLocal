using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class ColumnDescription
	{

		public readonly ColumnReference column;
		public readonly ColumnOptions options;
		
		public ColumnDescription(ColumnReference column, ColumnOptions options)
		{
			this.column = column;
			this.options = options;
		}
	
	}
}
