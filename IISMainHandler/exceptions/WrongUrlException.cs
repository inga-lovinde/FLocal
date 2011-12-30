using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;

namespace FLocal.IISHandler {
	class WrongUrlException : FLocalException {

		public WrongUrlException() : base("wrong url") {
		}

	}
}
