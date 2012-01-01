using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class TableDescription
	{

		public readonly string table;
		public readonly ColumnDescription primaryKey;
		public readonly ColumnDescription[] columns;

		public TableDescription(string table, ColumnDescription primaryKey, params ColumnDescription[] columns) {
			this.table = table;
			this.primaryKey = primaryKey;
			this.columns = columns;
		}

		public TableDescription(string table, ColumnDescription primaryKey, IEnumerable<ColumnDescription> columns)
			: this(table, primaryKey, columns.ToArray()) {
		}

	}
}
