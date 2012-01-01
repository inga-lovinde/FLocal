using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher
{
	class FormattableException : ApplicationException
	{
	
		public FormattableException(string message, params object[] data)
		: base(string.Format(message, data))
		{
		}
	
	}
}
