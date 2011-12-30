using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Poll : AbstractLocalLink {

		public Poll()
			: base("poll") {
		}

		protected override FLocal.Common.URL.AbstractUrl url {
			get {
				return new URL.polls.Info(this.DefaultOrValue, null);
			}
		}

	}
}
