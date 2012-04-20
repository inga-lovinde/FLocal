using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJonDevelopment.BBCode;

namespace FLocal.Common.BBCodes {
	class UploadImage : BBCode {

		public UploadImage()
			: base("uploadimage") {
		}

		public override string Format(IPostParsingContext context, ITextFormatter formatter) {
			var upload = dataobjects.Upload.LoadById(int.Parse(this.InnerText));
			var name = upload.filename;
			return "<f:img><f:src>/Upload/Item/" + upload.id.ToString() + "/</f:src><f:alt>" + this.Safe(upload.filename) + "</f:alt></f:img>";
		}

	}
}
