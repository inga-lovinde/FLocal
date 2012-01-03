using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NConsoler;
using FLocal.Common;

namespace FLocal.Migration.Console {
	class Program {
		public static void Main(string[] args) {
			Consolery.Run(typeof(Program), args);
		}

		private static bool isConfigInitialized = false;
		private static object initializeConfig_locker = new object();
		private static void initializeConfig() {
			if(!isConfigInitialized) {
				lock(initializeConfig_locker) {
					if(!isConfigInitialized) {
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
				System.Console.WriteLine(e.GetType().FullName + ": " + e.Message);
				System.Console.WriteLine(e.StackTrace);
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
