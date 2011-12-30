using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLConnector {
	class ParamsHolder {

		public Dictionary<string, string> data = new Dictionary<string,string>();

		private int index = 1;

		public string Add(string value) {
			lock(this) {
				string name = "param" + index;
				this.data.Add(name, value);
				this.index++;
				return name;
			}
		}

	}
}
