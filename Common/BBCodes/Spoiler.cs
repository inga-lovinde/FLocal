﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class Spoiler : BBCode {

		public Spoiler()
			: base("spoiler") {
		}

		public override string Format(ITextFormatter formatter) {
			string marker = this.Default;
			if(marker == null) marker = "Spoiler";
			return "<blockquote spoiler><font opener class=\"small\" onClick=\"showSpoiler(this)\">" + marker + "</font><hr/><div inner name=\"inner\">" + this.GetInnerHTML(formatter).Trim() + "</div><hr/></blockquote><br/>";
		}

	}
}
