using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Web.Core;

namespace Patcher {

	class Logger : ILogger {

		public static string LogDir {
			set {
				lock(_instance.locker) {
					_instance.writer = new StreamWriter(Path.Combine(value, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".patcher.txt"));
				}
			}
		}

		private static readonly Logger _instance = new Logger();
		public static readonly ILogger instance = _instance;

		private StreamWriter writer;

		private readonly object locker = new object();

		private Logger() {
		}

		void ILogger.Log(string message) {
			lock(this.locker) {
				this.writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + ": " + message);
				this.writer.Flush();
			}
		}

	}

}
