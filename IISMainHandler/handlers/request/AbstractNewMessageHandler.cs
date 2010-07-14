using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.IISHandler.handlers.request {
	abstract class AbstractNewMessageHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/MessageCreated.xslt";
			}
		}

		protected string getTitle(WebContext context) {
			string title = context.httprequest.Form["title"].Trim();
			if(title == "") {
				throw new FLocalException("Title is empty");
			}
			if(title.Length > 100) {
				throw new FLocalException("Title is too long");
			}
			return title;
		}

		protected string getBody(WebContext context) {
			string body = context.httprequest.Form["Body"].Trim();
			if(body == "") {
				throw new FLocalException("Body is empty");
			}
			if(body.Length > 30000) {
				throw new FLocalException("Body is too long");
			}
			return body;
		}

	}
}
