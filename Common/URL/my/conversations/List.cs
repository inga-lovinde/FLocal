using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my.conversations {
	public class List : AbstractUrl {
		
		public List(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Conversations";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Conversations/List/";
			}
		}
	}
}
