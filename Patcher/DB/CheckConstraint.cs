using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class CheckConstraint : AbstractConstraint
	{
	
		public override void Accept(IVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public override T Accept<T>(IVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}
		
		public readonly string condition;
		
		public CheckConstraint(string table, string name, string condition)
		: base(table, name)
		{
			this.condition = condition;
		}

	}
}
