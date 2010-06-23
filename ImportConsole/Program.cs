﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NConsoler;
using FLocal.Common;

namespace FLocal.ImportConsole {
	class Program {
		public static void Main(string[] args) {

			if(!Config.isInitialized) {
				lock(typeof(Config)) {
					if(!Config.isInitialized) {
						Config.Init(ConfigurationManager.AppSettings);
					}
				}
				Consolery.Run(typeof(Program), args);
			}
		}

		[Action]
		public static void ImportUsers() {
			UsersImporter.ImportUsers();
		}

		[Action]
		public static void ProcessUpload(string pathToUpload) {
			UploadProcessor.ProcessUpload(pathToUpload);
		}
	}
}
