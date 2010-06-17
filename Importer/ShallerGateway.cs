using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Importer {
	public class ShallerGateway {

		public static string getUserInfo(string userName) {
			return ShallerConnector.getPageContent("showprofile.php?User=" + userName + "&What=login&showlite=l", new Dictionary<string,string>(), new System.Net.CookieContainer());
		}

	}
}
