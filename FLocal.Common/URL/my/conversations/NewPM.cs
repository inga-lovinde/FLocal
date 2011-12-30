using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my.conversations {
	public class NewPM : AbstractUrl {

		public NewPM(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "New PM";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Conversations/NewPM/";
			}
		}
	}
}
