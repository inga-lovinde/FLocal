using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {

	class CreatePollHandler : AbstractGetHandler<FLocal.Common.URL.polls.New> {

		override protected string templateName {
			get {
				return "NewPoll.xslt";
			}
		}

		override protected IEnumerable<XElement>  getSpecificData(WebContext context) {
			return new XElement[] {
			};
		}
	}

}