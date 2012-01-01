using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher
{
	public class XmlResourceNotFoundException : ApplicationException
	{
	
		public XmlResourceNotFoundException(Uri absoluteUri)
		: base("XmlResource with uri '" + absoluteUri.ToString() + "' could not be found")
		{
		}
	
	}
}
