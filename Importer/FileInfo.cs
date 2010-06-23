using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FLocal.Importer {
	public struct FileInfo {

		public Stream dataStream;
		public string filePath;
		public string fileName;
		public long fileSize;
		public DateTime lastModified;

	}
}
