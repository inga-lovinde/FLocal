using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	public class Diapasone {

		public readonly long start;
		public readonly long count;
		public long total {
			get;
			set;
		}

		public Diapasone(long start, long count) {
			this.start = start;
			this.count = count;
		}

		public static Diapasone unlimited {
			get {
				return new Diapasone(0, -1);
			}
		}

		public static Diapasone first {
			get {
				return new Diapasone(0, 1);
			}
		}

	}
}
