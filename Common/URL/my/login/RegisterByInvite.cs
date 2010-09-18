using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my.login {
	public class RegisterByInvite : AbstractUrl {

		public RegisterByInvite(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Register by invite";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Login/RegisterByInvite/";
			}
		}
	}
}
