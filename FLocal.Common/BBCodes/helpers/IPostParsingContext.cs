using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLocal.Common.BBCodes {
	public interface IPostParsingContext {
		void OnUserMention(dataobjects.User user);
	}
}
