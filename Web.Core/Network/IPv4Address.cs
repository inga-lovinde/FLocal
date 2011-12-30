using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.Network {
	public class IPv4Address {

		private const ulong MAX = IPv4.UNIT * IPv4.UNIT * IPv4.UNIT * IPv4.UNIT;

		private readonly ulong raw;

		public IPv4Address(string ip) {
			string[] parts = ip.Split('.');
			if(parts.Length != 4) throw new ApplicationException("Malformed ip '" + ip + "'");
			ulong result = 0;
			for(int i=0; i<parts.Length; i++) {
				ulong part = ulong.Parse(parts[i]);
				if(part >= IPv4.UNIT) throw new ApplicationException("Malformed ip '" + ip + "'");
				result = (result*IPv4.UNIT) + part;
			}
			this.raw = result;
		}

		public IPv4Address(ulong raw) {
			if(raw >= MAX) throw new ApplicationException("Wrong raw representation " + raw);
			this.raw = raw;
		}

		public override string ToString() {
			string[] parts = new string[4];
			ulong temp = this.raw;
			for(int i=3; i>=0; i--) {
				ulong part = temp % IPv4.UNIT;
				parts[i] = part.ToString();
				temp = (temp - part) / IPv4.UNIT;
			}
			return String.Join(".", parts);
		}

		public IEnumerable<IPv4Subnet> matchingSubnets {
			get {
				ulong divisor = 1;
				for(int i=32; i>=0; i--) {
					yield return new IPv4Subnet(new IPv4Address(this.raw - (this.raw % divisor)), (byte)i);
					divisor *= 2;
				}
			}
		}

	}
}
