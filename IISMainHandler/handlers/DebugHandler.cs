using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler.handlers {
    class DebugHandler : ISpecificHandler {

        private HttpContext context;

        public DebugHandler(HttpContext context) {
            this.context = context;
        }

        public void Handle() {
            context.Response.ContentType = "text/plain";
            context.Response.WriteLine("Path: " + context.Request.Path);
            context.Response.WriteLine("PathInfo: " + context.Request.PathInfo);
        }

        public void Dispose() {
            this.context = null;
        }

    }
}
