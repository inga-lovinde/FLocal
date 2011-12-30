﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	interface IDesign : FLocal.Common.IOutputParams {

		string GetFSName(string template);

		string ContentType {
			get;
		}

		bool IsHuman {
			get;
		}

	}
}
