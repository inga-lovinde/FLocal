using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core.DB {
	class Diapasone {

		public readonly int start;
		public readonly int count;
		public int total {
			get;
			private set;
		}

		public Diapasone(int start, int count) {
			this.start = start;
			this.count = count;
		}

	}
}
