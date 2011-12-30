using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Web.Core;
using FLocal.Common;
using FLocal.Common.dataobjects;

namespace FLocal.Migration.Console {
	class UploadProcessor {
		public static void ProcessUpload(string pathToUpload) {
			User uploader = User.LoadByName("Guest 127.0.0.1");
			DirectoryInfo directoryInfo = new DirectoryInfo(pathToUpload);
			int i=0;
			foreach(FileSystemInfo _info in directoryInfo.GetFiles()) {
				if(i%100 == 0) {
					System.Console.Write("[" + (int)(i/100) + "]");
				}
				FileInfo info = _info as FileInfo;
				//Console.WriteLine("Processing " + info.FullName);
				if(!info.Name.StartsWith("file")) {
					System.Console.Write("!");
				} else {
					string[] parts = info.Name.Split('.');
					if(parts.Length != 2) throw new FLocalException("wrong file name");
					int raw = int.Parse(parts[0].PHPSubstring(4));
					int id;
					switch(parts[1].ToLower()) {
						case "jpg":
							id = raw;
							break;
						case "gif":
							id = 500000 + raw;
							break;
						case "png":
							id = 600000 + raw;
							break;
						default:
							throw new FLocalException("wrong extension");
					}
					if(info != null) {
						try {
							Upload.LoadById(id);
							System.Console.Write("-");
						} catch(NotFoundInDBException) {
							try {
								UploadManager.UploadFile(
									info.OpenRead(),
									info.Name,
									info.LastWriteTime,
									uploader,
									id
								);
							} catch(UploadManager.AlreadyUploadedException e) {
								System.Console.WriteLine(id + " md5 is equal to that of " + e.uploadId);
								System.Console.ReadLine();
							} catch(Exception e) {
								System.Console.WriteLine(e.GetType().FullName + ": " + e.Message);
								System.Console.WriteLine(e.StackTrace);
								throw;
							}
							System.Console.Write("+");
							//Console.WriteLine("Processed " + info.FullName);
						}
					}
				}
				i++;
			}
		}
	}
}
