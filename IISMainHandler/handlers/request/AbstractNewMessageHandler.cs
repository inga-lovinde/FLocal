using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;

namespace FLocal.IISHandler.handlers.request {
	abstract class AbstractNewMessageHandler : AbstractPostHandler {

		protected string getTitle(WebContext context) {
			string title = context.httprequest.Form["title"].Trim();
			if(title == "") {
				throw new FLocalException("Title is empty");
			}
			return title;
		}

		protected string getBody(WebContext context) {
			string body = context.httprequest.Form["body"].Trim();
			if(body == "") {
				throw new FLocalException("Body is empty");
			}
			return body;
		}

	}
}
