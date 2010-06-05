using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler.handlers {
    class WrongUrlHandler : ISpecificHandler  {

        public void Handle() {
            throw new HttpException(404, "page not found");
        }

        public void Dispose() { }

    }
}
