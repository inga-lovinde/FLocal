using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NConsoler;
using FLocal.Common;

namespace FLocal.ImportConsole {
	class Program {
		public static void Main(string[] args) {
			Consolery.Run(typeof(Program), args);
		}

		private static void initializeConfig() {
			if(!Config.isInitialized) {
				lock(typeof(Config)) {
					if(!Config.isInitialized) {
						Config.Init(ConfigurationManager.AppSettings);
					}
				}
			}
		}

		[Action]
		public static void ImportUsers() {
			initializeConfig();
			try {
				UsersImporter.ImportUsers();
			} catch(Exception e) {
				Console.WriteLine(e.GetType().FullName + ": " + e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}

		[Action]
		public static void ProcessUpload(string pathToUpload) {
			initializeConfig();
			UploadProcessor.ProcessUpload(pathToUpload);
		}

		[Action]
		public static void ConvertThreaded(string pathToThreaded, string outFile) {
			ThreadedHTMLProcessor.Process(pathToThreaded, outFile);
		}

		[Action]
		public static void ImportShallerDB(string pathToDB) {
			initializeConfig();
			ShallerDBProcessor.processDB(pathToDB);
		}
	}
}
