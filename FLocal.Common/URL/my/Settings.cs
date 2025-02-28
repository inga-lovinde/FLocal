﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.URL.my {
	public class Settings : AbstractUrl {

		public Settings(string remainder) : base(remainder) {
		}

		public override string title {
			get {
				return "Settings";
			}
		}

		protected override string _canonical {
			get {
				return "/My/Settings/";
			}
		}
	}
}
