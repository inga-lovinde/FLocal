using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.users {
	public class Active : AbstractUrl {

		public Active(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Active users";
			}
		}

		protected override string _canonical {
			get {
				return "/Users/Active/";
			}
		}

	}
}
