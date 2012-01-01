using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class ForeignKeyConstraint : AbstractConstraint
	{
	
		public override void Accept(IVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}
		
		public enum ReferentialAction
		{
			NoAction,
			Cascade,
			SetNull,
			SetDefault,
		}

		public readonly string column;
		public readonly string referencedTable;
		public readonly ReferentialAction onUpdate;
		public readonly ReferentialAction onDelete;
		
		public ForeignKeyConstraint(string table, string name, string column, string referencedTable, ReferentialAction onUpdate, ReferentialAction onDelete)
		: base(table, name)
		{
			this.column = column;
			this.referencedTable = referencedTable;
			this.onUpdate = onUpdate;
			this.onDelete = onDelete;
		}

	}
}
