using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class ColumnReference
	{

		public readonly string tableName;
		public readonly string columnName;
		
		public ColumnReference(string tableName, string columnName)
		{
			this.tableName = tableName;
			this.columnName = columnName;
		}
	
	}
}
