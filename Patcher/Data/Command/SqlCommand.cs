using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	class SqlCommand : AbstractCommand
	{

		private readonly string[] installQueries;
		private readonly string[] uninstallQueries;
	
		private SqlCommand(int num, XElement inner) : base(num)
		{
			this.installQueries = (from elem in inner.Element("installSql").Elements() select elem.Value).ToArray();
			this.uninstallQueries = (from elem in inner.Elements("uninstallSql").Elements() select elem.Value).ToArray();
		}
		
		public static SqlCommand CreateSpecific(int num, XElement inner)
		{
			return new SqlCommand(num, inner);
		}

		public override IEnumerable<XElement> Apply(Transaction transaction)
		{
			for(int i=0; i<this.installQueries.Length; i++)
			{
				this.Execute(this.installQueries[i], transaction);
			}
			return Enumerable.Empty<XElement>();
		}

		public override void Rollback(Transaction transaction, XElement commandRollbackInfo)
		{
			for(int i=0; i<this.uninstallQueries.Length; i++)
			{
				this.Execute(this.uninstallQueries[i], transaction);
			}
		}
	}
}
