using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FLocal.Core;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.request {
	class VoteHandler : AbstractPostHandler {

		protected override string templateName {
			get {
				return "result/VoteAccepted.xslt";
			}
		}

		protected override XElement[] Do(WebContext context) {

			Poll poll = Poll.LoadById(int.Parse(context.httprequest.Form["pollId"]));

			string[] rawOptions = context.httprequest.Form.GetValues("option");
			HashSet<int> options = new HashSet<int>();
			foreach(string rawOption in rawOptions) {
				options.Add(int.Parse(rawOption));
			}

			if(!poll.isMultiOption && options.Count > 1) {
				throw new FLocalException(options.Count + " options selected in a single-option poll");
			}

			poll.GiveVote(context.session.account.user, options);

			return new XElement[] {
				poll.exportToXml(context),
			};
		}
	}
}
