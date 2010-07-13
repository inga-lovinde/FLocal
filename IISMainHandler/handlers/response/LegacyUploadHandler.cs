﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;

namespace FLocal.IISHandler.handlers.response {
	class LegacyUploadHandler : RedirectGetHandler {

		protected override string getRedirectUrl(WebContext context) {
			if(context.requestParts.Length != 3) throw new WrongUrlException();// throw new FLocalException("wrong url");
			string[] parts = context.requestParts[2].Split('.');
			if(parts.Length != 2) throw new WrongUrlException();// throw new FLocalException("wrong url");
			if(parts[0].PHPSubstring(0, 4).ToLower() != "file") throw new WrongUrlException();// throw new FLocalException("wrong url");
			int rawFileNum = int.Parse(parts[0].PHPSubstring(4));
			int fileNum;
			switch(parts[1].ToLower()) {
				case "jpg":
					fileNum = rawFileNum;
					break;
				case "gif":
					fileNum = 500000 + rawFileNum;
					break;
				case "png":
					fileNum = 600000 + rawFileNum;
					break;
				default:
					//throw new FLocalException("wrong url");
					throw new WrongUrlException();
			}
			return "/Upload/Item/" + fileNum + "/";
		}

	}
}
