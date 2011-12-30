using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {

    public partial class AccessDeniedException : FLocalException {

        public AccessDeniedException(string Message) : base(Message) { }

		public AccessDeniedException() : this("Access denied") { }

    }

}
