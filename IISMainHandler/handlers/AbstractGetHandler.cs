using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.handlers {
    abstract class AbstractGetHandler : ISpecificHandler {
        
        abstract protected string templateName {
            get;
        }

        abstract protected System.Xml.Linq.XDocument getData(WebContext context);

        public void Handle(WebContext context) {
            context.httpresponse.Write(context.Transform(this.templateName, this.getData(context)));
        }

    }
}
