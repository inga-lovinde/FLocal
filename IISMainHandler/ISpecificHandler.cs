using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler {
    interface ISpecificHandler {

        void Handle(WebContext context);

    }
}
