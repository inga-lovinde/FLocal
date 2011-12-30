using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.maintenance {
	public class LocalNetworks : AbstractUrl {

		public LocalNetworks(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Local networks";
			}
		}

		protected override string _canonical {
			get {
				return "/Maintenance/LocalNetworks/";
			}
		}
	}
}
