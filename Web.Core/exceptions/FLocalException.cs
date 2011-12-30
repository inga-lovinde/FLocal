using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {

    public partial class FLocalException : ApplicationException {

        public readonly string FullStackTrace;

        public FLocalException(string Message) : base(Message) {
            this.FullStackTrace = Environment.StackTrace;
        }

        public string getMessage() {
            return this.Message;
        }

        public override string StackTrace {
            get {
                return FullStackTrace;
            }
        }

    }

}
