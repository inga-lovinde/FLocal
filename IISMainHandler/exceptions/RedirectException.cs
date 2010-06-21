using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler {
	class RedirectException : FLocal.Core.FLocalException {

		public readonly string newUrl;

		public RedirectException(string newUrl) : base("Redirect to " + newUrl + " is being performed") {
			this.newUrl = newUrl;
		}

	}
}
