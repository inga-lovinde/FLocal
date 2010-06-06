using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace FLocal.Common {

	public class Config : FLocal.Core.Config<Config> {

		public readonly string InitTime;

		protected Config(NameValueCollection data) : base(data) {
			this.InitTime = DateTime.Now.ToLongTimeString();
		}

		public static void Init(NameValueCollection data) {
			doInit(() => new Config(data));
		}

		public static void ReInit(NameValueCollection data) {
			doReInit(() => new Config(data));
		}

	}

}
