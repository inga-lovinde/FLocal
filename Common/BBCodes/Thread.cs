using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Thread : BBCode {

		public Thread()
			: base("thread") {
		}

		public override string Format(ITextFormatter formatter) {
			var thread = dataobjects.Thread.LoadById(int.Parse(this.DefaultOrValue));
			var name = this.Safe(thread.title);
			if(this.Default != null) {
				name = this.GetInnerHTML(formatter);
			}
			return "<a href=\"/Thread/" + thread.id.ToString() + "/\">" + name + "</a>";
		}

	}
}
