using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.dataobjects {
	public interface IUserSettings {

		int threadsPerPage {
			get;
		}

		int postsPerPage {
			get;
		}

		int usersPerPage {
			get;
		}

	}
}
