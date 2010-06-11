using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common {
	abstract public class UserContext {
	
		public Common.Config config {
			get {
				return Common.Config.instance;
			}
		}

		abstract public IOutputParams outputParams {
			get;
		}

		abstract public dataobjects.IUserSettings userSettings {
			get;
		}

	}

}
