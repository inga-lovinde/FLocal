﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	class Rss : IDesign {

		public string GetFSName(string template) {
			return System.IO.Path.Combine("Rss", template);
		}

		string FLocal.Common.IOutputParams.preprocessBodyIntermediate(string bodyIntermediate) {
			return bodyIntermediate;
		}

		public string ContentType {
			get {
				return "application/rss+xml";
			}
		}

		public bool IsHuman {
			get {
				return false;
			}
		}

	}
}
