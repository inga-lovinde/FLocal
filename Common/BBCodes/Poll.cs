using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Poll : BBCode {

		public Poll()
			: base("poll") {
		}

		public override string Format(ITextFormatter formatter) {
			var poll = dataobjects.Poll.LoadById(int.Parse(this.DefaultOrValue));
			var name = poll.title;
			if(this.Default != null) {
				name = this.GetInnerHTML(formatter);
			}
			return "<a href=\"/Poll/" + poll.id.ToString() + "/\">" + name + "</a>";
		}

	}
}
