using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher
{
	public class ResourceNotFoundException : ApplicationException
	{
	
		public ResourceNotFoundException(string name)
		: base("Cannot read resource " + name)
		{
		}
	
	}
}
