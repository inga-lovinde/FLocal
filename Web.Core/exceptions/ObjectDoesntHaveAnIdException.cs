﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Core {

    public partial class ObjectDoesntHaveAnIdException : FLocalException {

        public ObjectDoesntHaveAnIdException()
            : base("Object doesn't have an id") {
        }

    }

}
