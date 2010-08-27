using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.Network {
	public class IPv4Subnet {

		private readonly IPv4Address prefix;
		private readonly byte length;
 
		public IPv4Subnet(IPv4Address prefix, byte length) {
			if(length > 32) throw new CriticalException("Wrong length " + length);
			this.prefix = prefix;
			this.length = length;
		}

		public static IPv4Subnet FromString(string subnet) {
			string[] parts = subnet.Split('/');
			if(parts.Length != 2) throw new ApplicationException("Malformed subnet '" + subnet + "'");
			return new IPv4Subnet(new IPv4Address(parts[0]), byte.Parse(parts[1]));
		}

		public override string ToString() {
			return this.prefix.ToString() + "/" + this.length;
		}

	}
}
