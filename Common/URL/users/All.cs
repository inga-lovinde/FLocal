﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.users {
	public class All : AbstractUrl {

		public All(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "All users";
			}
		}

		protected override string _canonical {
			get {
				return "/Users/All/";
			}
		}

	}
}
