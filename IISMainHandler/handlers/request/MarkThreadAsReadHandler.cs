using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Common.dataobjects;
using FLocal.Importer;
using System.Text.RegularExpressions;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.actions;
using System.Web;

namespace FLocal.IISHandler.handlers.request {
	class MarkThreadAsReadHandler : ReturnPostHandler {

		protected override void _Do(WebContext context) {
			Account account = context.session.account;
			Thread thread = Thread.LoadById(int.Parse(context.requestParts[2]));
			if(!context.requestParts[3].StartsWith("p")) throw new CriticalException("wrong url");
			Post post = Post.LoadById(int.Parse(context.requestParts[3].PHPSubstring(1)));

			if(post.thread.id != thread.id) throw new CriticalException("id mismatch");

			thread.forceMarkAsRead(account, post);
		}

	}
}
