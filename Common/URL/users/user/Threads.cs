using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.users.user {
	public class Threads : Abstract {

		public Threads(string userId, string remainder) : base(userId, remainder) {
		}

		protected override string _canonical {
			get {
				return "/Users/User/" + this.user.id + "/Threads/";
			}
		}
	}
}
