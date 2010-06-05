using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Core {

    partial class ObjectDoesntHaveAnIdException : FLocalException {

        public ObjectDoesntHaveAnIdException()
            : base("Object doesn't have an id") {
        }

    }

}
