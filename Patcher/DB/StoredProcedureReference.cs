using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class StoredProcedureReference
	{
	
		public enum ParameterDirection
		{
			In,
			Out,
		}

		public class ParameterDescription
		{
		
		
			public readonly string name;
			public readonly ParameterDirection direction;
			public readonly string type;
			
			public ParameterDescription(string name, ParameterDirection direction, string type)
			{
				this.name = name;
				this.direction = direction;
				this.type = type;
			}
			
		}

		public readonly string packageName;
		public readonly string procedureName;
		public readonly ParameterDescription[] parameters;
		
		public StoredProcedureReference(string packageName, string procedureName, params ParameterDescription[] parameters)
		{
			this.packageName = packageName;
			this.procedureName = procedureName;
			this.parameters = parameters;
		}
		
		public StoredProcedureReference(string packageName, string procedureName, IEnumerable<ParameterDescription> parameters)
		: this(packageName, procedureName, parameters.ToArray())
		{
		}
	
	}
}
