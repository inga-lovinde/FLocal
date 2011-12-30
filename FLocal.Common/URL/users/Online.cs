using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.users {
	public class Online : AbstractUrl {

		public Online(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Who is online";
			}
		}

		protected override string _canonical {
			get {
				return "/Users/Online/";
			}
		}

	}
}
