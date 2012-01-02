using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Core;

namespace MySQLConnector {
	class CommandExecutionLogger : IDisposable {

		private readonly ILogger logger;

		private readonly DateTime start;

		public string commandText;

		public CommandExecutionLogger(ILogger logger) {
			this.logger = logger;
			this.start = DateTime.Now;
		}

		void IDisposable.Dispose() {
			this.logger.Log("Spent " + (DateTime.Now-start).TotalSeconds + " seconds while executing " + this.commandText);
		}

	}
}
