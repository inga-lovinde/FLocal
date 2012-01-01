using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Patcher.DB;

namespace Patcher.Data.Command
{
	abstract class AbstractStoredProcedureCommand : AbstractPersistentCommand
	{

		private static readonly Dictionary<string, StoredProcedureReference.ParameterDirection> directions = new Dictionary<string, StoredProcedureReference.ParameterDirection>
		                                                                                                       {
		                                                                                                       	{ "in", StoredProcedureReference.ParameterDirection.In },
		                                                                                                       	{ "out", StoredProcedureReference.ParameterDirection.Out },
		                                                                                                       };

		protected readonly StoredProcedureReference procedure;
	
		protected AbstractStoredProcedureCommand(int num, XElement inner) : base(num)
		{
			XElement description = inner.Element("procedure");
			this.procedure = new StoredProcedureReference(
				description.Element("package").Value,
				description.Element("name").Value,
				from elem in description.Elements("parameter") select new StoredProcedureReference.ParameterDescription(
					elem.Element("name").Value,
					directions[elem.Element("direction").Value],
					elem.Element("type").Value
				)
			);
		}
		
	}
}
