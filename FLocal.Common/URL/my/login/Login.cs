using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my.login {
	public class Login : AbstractUrl {

		public Login(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Login";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Login/Login/";
			}
		}
	}
}
