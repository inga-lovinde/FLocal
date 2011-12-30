using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;

namespace FLocal.Common.URL {
	public abstract class AbstractUrl {

		protected AbstractUrl(string remainder) {
			this.remainder = remainder;
		}

		public string remainder {
			get;
			private set;
		}

		abstract public string title {
			get;
		}

		abstract protected string _canonical {
			get;
		}

		public string canonical {
			get {
				string result = this._canonical;
				//if(!result.StartsWith("/")) result = "/" + result;
				//if(!result.EndsWith("/")) result = result + "/";
				return result;
			}
		}

		public string canonicalFull {
			get {
				if(this.remainder == null) throw new CriticalException("Remainder is null");
				return this.canonical + this.remainder;
			}
		}

	}
}
