using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.actions {
	abstract public class AbstractFieldValue {

		abstract public string getStringRepresentation();
		abstract public string getStringRepresentation(string oldInfo);

	}
}
