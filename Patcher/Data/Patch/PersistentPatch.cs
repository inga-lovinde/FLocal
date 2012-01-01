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
	class PersistentPatch : AbstractPatch
	{

		private readonly AbstractCommand command;
	
		public PersistentPatch(AbstractCommand command, HashSet<string> restrictToEnvironments, Context context) : base(restrictToEnvironments, context)
		{
			this.command = command;
			throw new NotImplementedException("Persistent patches are not implemented yet");
		}

		public override XDocument Apply(Transaction transaction)
		{
			throw new NotImplementedException("Persistent patches are not implemented yet");
		}
		
		public override void Rollback(Transaction transaction, XDocument rollbackInfo)
		{
			throw new NotImplementedException("Persistent patches are not implemented yet");
		}
	
	}
}
