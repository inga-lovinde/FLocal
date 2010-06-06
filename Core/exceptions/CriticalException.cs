using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

    public partial class CriticalException : FLocalException {

        public CriticalException(string Message) : base(Message) { }

    }

}
