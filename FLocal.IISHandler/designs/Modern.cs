﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	class Modern : IDesign {

		public string GetFSName(string template) {
			return System.IO.Path.Combine("Modern", template);
		}

		string FLocal.Common.IOutputParams.preprocessBodyIntermediate(string bodyIntermediate) {
			return bodyIntermediate.
				Replace("<f:img><f:src>", "<img class=\"uploadImage\" src=\"").
				Replace("</f:src><f:alt>", "\" alt=\"").
				Replace("</f:alt></f:img>", "\"/>");
		}

		public string ContentType {
			get {
				return "text/html";
			}
		}

		public bool IsHuman {
			get {
				return true;
			}
		}

	}
}
