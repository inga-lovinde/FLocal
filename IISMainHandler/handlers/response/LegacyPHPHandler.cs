using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLocal.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers.response {
	class LegacyPHPHandler : RedirectGetHandler {

		protected override string getRedirectUrl(WebContext context) {
			string[] scriptParts = context.requestParts[0].Split('.');
			if(scriptParts.Length != 2) {
				//throw new FLocalException("wrong url");
				throw new WrongUrlException();
			}
			if(scriptParts[1].ToLower() != "php") {
				//throw new FLocalException("wrong url");
				throw new WrongUrlException();
			}

			switch(scriptParts[0].ToLower()) {
				case "showflat":
				case "ashowflat":
					Post post = Post.LoadById(int.Parse(context.httprequest.QueryString["Number"]));
					return "/Thread/" + post.thread.id.ToString() + "/p" + post.id.ToString();
				case "showthreaded":
				case "ashowthreaded":
					return "/Post/" + int.Parse(context.httprequest.QueryString["Number"]).ToString() + "/";
				case "postlist":
					return "/Board/" + Board.LoadByLegacyName(context.httprequest.QueryString["Board"]).id.ToString() + "/";
				case "showprofile":
					return "/User/" + User.LoadByName(context.httprequest.QueryString["User"]).id.ToString() + "/";
				default:
					//throw new NotImplementedException("unknown script " + scriptParts[0]);
					throw new WrongUrlException();
			}
		}

	}
}
