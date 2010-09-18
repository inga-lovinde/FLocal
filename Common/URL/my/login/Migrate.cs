using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my.login {
	public class Migrate : AbstractUrl {

		public Migrate(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Migrate";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Login/Migrate/";
			}
		}
	}
}
