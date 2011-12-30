using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.upload {
	public class New : AbstractUrl {

		public New(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Upload new file";
			}
		}

		protected override string _canonical {
			get {
				return "/Upload/New/";
			}
		}
	}
}
