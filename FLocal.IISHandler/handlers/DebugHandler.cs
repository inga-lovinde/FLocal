﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Web.Core.DB;
using FLocal.Common.dataobjects;

namespace FLocal.IISHandler.handlers {
	class DebugHandler : ISpecificHandler {

		private string type;

		public DebugHandler(string type) {
			this.type = type;
		}

		public void Handle(WebContext context) {
			context.httpresponse.ContentType = "text/plain";
			context.httpresponse.WriteLine("Page: " + this.type);
			context.httpresponse.WriteLine("Path: " + context.httprequest.Path);
			context.httpresponse.WriteLine("PathInfo: " + context.httprequest.PathInfo);
			context.httpresponse.WriteLine("AppInfo: " + context.config.AppInfo);
			context.httpresponse.WriteLine("InitTime: " + context.config.InitTime);
			if(context.httprequest.Path == "/test/") {
				using(Transaction transaction = context.config.mainConnection.beginTransaction()) {
					context.httpresponse.WriteLine(transaction.GetHashCode().ToString());
				}
			}
			if(context.httprequest.Path == "/test2/") {
				Board board = Board.LoadById(1);
				context.httpresponse.WriteLine("name: " + board.name);
				context.httpresponse.WriteLine("description: " + board.description);
				context.httpresponse.WriteLine("categoryname: " + board.category.name);
			}

			Thread.LoadById(1).ReLoad();
		}

	}
}
