using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {
	public class Counter {

		private int value;
		private readonly object locker = new object();

		public Counter() {
			this.value = 0;
		}

		public int GetCurrentValueAndIncrement() {
			lock(this.locker) {
				int result = this.value;
				this.value++;
				return result;
			}
		}

	}
}
