using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Builder {
	class TempFile : IDisposable {

		public readonly string fileName;

		public TempFile() {
			this.fileName = Path.GetTempFileName();
		}

		public StreamReader getReader() {
			return new StreamReader(this.fileName);
		}

		public StreamWriter getWriter() {
			return new StreamWriter(this.fileName);
		}

		public void Dispose() {
			File.Delete(this.fileName);
		}

	}
}
