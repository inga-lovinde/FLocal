using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace FLocal.IISHandler.handlers {

    class RootHandler : AbstractGetHandler {

        override protected string templateName {
            get {
                throw new NotImplementedException();
            }
        }

        override protected XDocument getData(WebContext context) {
            throw new NotImplementedException();
        }

    }

}