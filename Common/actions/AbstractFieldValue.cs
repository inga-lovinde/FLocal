using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	abstract class AbstractFieldValue {

		abstract public string getStringRepresentation();
		abstract public string getStringRepresentation(string oldInfo);

	}
}
