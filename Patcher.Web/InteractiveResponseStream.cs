using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;
using System.Web;

namespace Patcher.Web {
	class InteractiveResponseStream : IInteractiveConsole {

		private readonly HttpContext context;

		public void Report(string message) {
			this.context.Response.Output.WriteLine(message);
		}

		public void Report(string format, params object[] data) {
			this.context.Response.Output.WriteLine(format, data);
		}

		public bool IsInteractive {
			get {
				return false;
			}
		}

		public bool Ask(string question) {
			throw new NotImplementedException();
		}

		public void WaitForUserAction() {
			throw new NotImplementedException();
		}

		public InteractiveResponseStream(HttpContext context) {
			this.context = context;
		}

	}
}
