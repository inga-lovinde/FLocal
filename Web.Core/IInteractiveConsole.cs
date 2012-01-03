using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {
	public interface IInteractiveConsole {

		void Report(string message);

		void Report(string format, params object[] data);

		bool IsInteractive {
			get;
		}

		bool Ask(string question);

		void WaitForUserAction();

	}
}
