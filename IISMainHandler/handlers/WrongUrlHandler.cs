using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FLocal.IISHandler.handlers {
	class WrongUrlHandler : ISpecificHandler  {

		public void Handle(WebContext context) {
			throw new HttpException(400, "wrong url");
		}

	}
}
