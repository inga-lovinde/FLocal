using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler {
    class HandlersFactory {

        public static ISpecificHandler getHandler(HttpContext context) {
            //return new handlers.DebugHandler(context);
            return new handlers.WrongUrlHandler();
        }

    }
}
