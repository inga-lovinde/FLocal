using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	abstract class AbstractConstraint
	{
	
		public interface IVisitor
		{
			void Visit(ForeignKeyConstraint constraint);
			void Visit(UniqueConstraint constraint);
			void Visit(CheckConstraint constraint);
		}
		
		public interface IVisitor<T>
		{
			T Visit(ForeignKeyConstraint constraint);
			T Visit(UniqueConstraint constraint);
			T Visit(CheckConstraint constraint);
		}
		
		private class Visitor : IVisitor
		{

			private Action<ForeignKeyConstraint> foreignKeyAction;
			private Action<UniqueConstraint> uniqueAction;
			private Action<CheckConstraint> checkAction;
		
			public Visitor(
				 Action<ForeignKeyConstraint> foreignKeyAction
				,Action<UniqueConstraint> uniqueAction
				,Action<CheckConstraint> checkAction
			)
			{
				this.foreignKeyAction = foreignKeyAction;
				this.uniqueAction = uniqueAction;
				this.checkAction = checkAction;
			}
			
			void IVisitor.Visit(ForeignKeyConstraint constraint)
			{
				this.foreignKeyAction(constraint);
			}

			public void Visit(UniqueConstraint constraint)
			{
				this.uniqueAction(constraint);
			}

			public void Visit(CheckConstraint constraint)
			{
				this.checkAction(constraint);
			}
		}

		private class Visitor<T> : IVisitor<T>
		{

			private Func<ForeignKeyConstraint, T> foreignKeyAction;
			private Func<UniqueConstraint, T> uniqueAction;
			private Func<CheckConstraint, T> checkAction;
		
			public Visitor(
				 Func<ForeignKeyConstraint, T> foreignKeyAction
				,Func<UniqueConstraint, T> uniqueAction
				,Func<CheckConstraint, T> checkAction
			)
			{
				this.foreignKeyAction = foreignKeyAction;
				this.uniqueAction = uniqueAction;
				this.checkAction = checkAction;
			}
			
			T IVisitor<T>.Visit(ForeignKeyConstraint constraint)
			{
				return this.foreignKeyAction(constraint);
			}

			public T Visit(UniqueConstraint constraint)
			{
				return this.uniqueAction(constraint);
			}

			public T Visit(CheckConstraint constraint)
			{
				return this.checkAction(constraint);
			}
		}

		abstract public void Accept(IVisitor visitor);
		abstract public T Accept<T>(IVisitor<T> visitor);
		public void Accept(
			 Action<ForeignKeyConstraint> foreignKeyAction
			,Action<UniqueConstraint> uniqueAction
			,Action<CheckConstraint> checkAction
		)
		{
			this.Accept(new Visitor(foreignKeyAction, uniqueAction, checkAction));
		}
		public T Accept<T>(
			 Func<ForeignKeyConstraint, T> foreignKeyFunc
			,Func<UniqueConstraint, T> uniqueFunc
			,Func<CheckConstraint, T> checkFunc
		)
		{
			return this.Accept(new Visitor<T>(foreignKeyFunc, uniqueFunc, checkFunc));
		}

		public readonly string table;
		public readonly string name;
		protected AbstractConstraint(string table, string name)
		{
			this.table = table;
			this.name = name;
		}
		
	}
}
