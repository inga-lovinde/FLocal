using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Common.dataobjects;

namespace FLocal.Common.URL {
	public class QuickLink : AbstractUrl {

		public readonly dataobjects.QuickLink link;

		public QuickLink(string quickLinkId, string remainder) : base(remainder) {
			this.link = dataobjects.QuickLink.LoadByName(quickLinkId);
		}

		public override string title {
			get {
				return this.link.name;
			}
		}

		protected override string _canonical {
			get {
				return "/q/" + this.link.name;
			}
		}
	}
}
