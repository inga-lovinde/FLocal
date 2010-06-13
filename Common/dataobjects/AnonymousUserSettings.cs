using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.dataobjects {
	public class AnonymousUserSettings : IUserSettings {

		public int threadsPerPage {
			get {
				return 40;
			}
		}

		public int postsPerPage {
			get {
				return 20;
			}
		}

	}
}
