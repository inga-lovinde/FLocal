using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class UniqueConstraint : AbstractConstraint
	{
	
		public override void Accept(IVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}
		
		public readonly HashSet<string> columns;
		
		public UniqueConstraint(string table, string name, HashSet<string> columns)
		: base(table, name)
		{
			this.columns = columns;
		}

	}
}
