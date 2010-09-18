using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my {
	public class UserData : AbstractUrl {

		public UserData(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Change user data";
			}
		}

		protected override string _canonical {
			get {
				return "/My/UserData/";
			}
		}
	}
}
