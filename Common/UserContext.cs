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

		abstract public string formatDateTime(DateTime dateTime);

	}

	public static class UserContext_Extensions {
		
		public static string ToString(this DateTime dateTime, UserContext context) {
			return context.formatDateTime(dateTime);
		}

	}

}
