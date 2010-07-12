using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class CreatePollHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/PollCreated.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			string title = context.httprequest.Form["title"].Trim();
			if(title == "") throw new FLocalException("title is empty");

			string[] rawOptions = context.httprequest.Form.GetValues("option");
			List<string> options = new List<string>();
			foreach(string rawOption in rawOptions) {
				if(rawOption != null && rawOption.Trim() != "") {
					options.Add(rawOption.Trim());
				}
			}
			if(options.Count < 2) throw new FLocalException("Only " + options.Count + " options is entered");

			bool isDetailed = context.httprequest.Form.AllKeys.Contains("isDetailed");
			bool isMultiOption = context.httprequest.Form.AllKeys.Contains("isMultiOption");

			Poll poll = Poll.Create(
				context.session.account.user,
				isDetailed,
				isMultiOption,
				title,
				options
			);
			return new XElement[] {
				poll.exportToXml(context),
			};
		}
	}
}
