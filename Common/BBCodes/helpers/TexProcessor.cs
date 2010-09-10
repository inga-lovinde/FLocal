using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.TexCompiler;
using System.IO;

namespace FLocal.Common.BBCodes.helpers {
	static class TexProcessor {

		public static dataobjects.Upload getCompiled(string tex) {
			dataobjects.User uploader = dataobjects.User.LoadByName("Mr. TeX compiler");
			using(Stream stream = Compiler.GetPngStream(tex)) {
				return UploadManager.SafeUploadFile(stream, "tex.png", uploader);
			}
		}

	}
}
