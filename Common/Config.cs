using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common {

	public class Config : FLocal.Core.Config<Config> {

		protected Config() : base() {
		}

		public static void Init() {
			doInit(() => new Config());
		}

	}

}
