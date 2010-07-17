using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.IISHandler.designs {
	interface IDesign : Common.IOutputParams {

		string GetFSName(string template);

	}
}
