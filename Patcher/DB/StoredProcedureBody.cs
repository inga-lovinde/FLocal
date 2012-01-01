using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.DB
{
	class StoredProcedureBody
	{

		public readonly string declarations;
		public readonly string body;
		
		public StoredProcedureBody(string declarations, string body)
		{
			this.declarations = declarations;
			this.body = body;
		}
		
	}
}
