﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class UploadLink : BBCode {

		public UploadLink()
			: base("uploadlink") {
		}

		public override string Format(ITextFormatter formatter) {
			var upload = dataobjects.Upload.LoadById(int.Parse(this.DefaultOrValue));
			var name = this.Safe(upload.filename);
			if(this.Default != null) {
				name = this.GetInnerHTML(formatter);
			}
			return "<a href=\"/Upload/Item/" + upload.id.ToString() + "/\">" + name + "</a>";
		}

	}
}
