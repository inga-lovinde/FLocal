using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Patcher
{
	class CultureReplacementWrapper : IDisposable
	{

		private readonly CultureInfo originalCulture;
		
		public CultureReplacementWrapper(CultureInfo newCulture)
		{
			this.originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
		}
		
		void IDisposable.Dispose()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = this.originalCulture;
		}

	}
}
