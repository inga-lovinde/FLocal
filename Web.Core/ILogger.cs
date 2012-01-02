using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {
	public interface ILogger {

		/// <summary>
		/// </summary>
		/// <param name="message">Single-line message</param>
		void Log(string message);

	}
}
