﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	class Raw : IDesign {

		public string GetFSName(string template) {
			return "copy.xslt";
		}

		string FLocal.Common.IOutputParams.preprocessBodyIntermediate(string bodyIntermediate) {
			return bodyIntermediate;
		}
	}
}
