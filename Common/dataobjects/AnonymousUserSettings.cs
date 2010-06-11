using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		public int threadsPerPage {
			get {
				return 20;
			}
		}

		public int postsPerPage {
			get {
				return 40;
			}
		}

	}
}
