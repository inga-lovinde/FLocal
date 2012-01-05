using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Patcher.Data.Command;
using Patcher.DB;

namespace Patcher.Data.Patch
{
	class AtomicPatch : AbstractPatch
	{

		private readonly AbstractCommand[] commands;
		private readonly bool isStrictCommandSet;
	
		public AtomicPatch(AbstractCommand[] commands, bool isStrictCommandSet, HashSet<string> restrictToEnvironments, Context context) : base(restrictToEnvironments, context)
		{
			this.commands = commands;
			this.isStrictCommandSet = isStrictCommandSet;
		}
		
		private void CheckDbDriver()
		{
			if(!this.isStrictCommandSet)
			{
				if(!context.DbDriver.IsDDLTransactional)
				{
					throw new ApplicationException("You should use strictCommandSet when working with non-DDL-transactional DB");
				}
			}
		}

		public override XDocument Apply(Transaction transaction)
		{
			this.CheckDbDriver();
		
			List<XElement> rollbackInfos = new List<XElement>();
			for(int i=0; i<this.commands.Length; i++)
			{
				IEnumerable<XElement> commandRollbackInfoContent;
				if (commands[i] is AbstractPersistentCommand)
				{
					commandRollbackInfoContent = ((AbstractPersistentCommand)commands[i]).Apply(transaction, true);
				} else
				{
					commandRollbackInfoContent = commands[i].Apply(transaction);
				}
				rollbackInfos.Add(new XElement("command", new XAttribute("num", i), commandRollbackInfoContent));
			}
			return new XDocument(new XElement("rollbackInfo", rollbackInfos.ToArray()));
		}
		
		public override void Rollback(Transaction transaction, XDocument rollbackInfo)
		{
			this.CheckDbDriver();
		
			for(int i=this.commands.Length-1; i>=0; i--)
			{
				commands[i].Rollback(transaction, (from commandRollbackInfo in rollbackInfo.Root.Elements("command") where commandRollbackInfo.Attribute("num").Value == i.ToString() select commandRollbackInfo).Single());
			}
		}
	
	}
}
