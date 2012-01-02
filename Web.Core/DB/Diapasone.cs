using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core.DB {
	public class Diapasone {

		public readonly long start;
		public readonly long count;

		/// <summary>
		/// Diapasone total value of null means that it has not yet been computed
		/// </summary>
		public long? total {
			get;
			set;
		}

		public Diapasone(long start, long count) {
			this.start = start;
			this.count = count;
			this.total = null;
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
