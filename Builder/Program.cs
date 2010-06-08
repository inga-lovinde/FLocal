using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Microsoft.Tools.WindowsInstallerXml;
//using System.Xml;
using System.Diagnostics;

namespace Builder {
	class Program {
		static void Main(string[] args) {

			const string WIXPATH = "C:\\Program Files (x86)\\Windows Installer XML v3\\bin\\";

			try {
				
				if(args.Length != 1) throw new ApplicationException("You should specify project name first");

				string path = args[0];
				if(!Directory.Exists(path)) throw new ApplicationException("Directory doesn't exists");
				string fullPath = new DirectoryInfo(path).FullName;
				fullPath += Path.DirectorySeparatorChar;

				string sourceFile = fullPath + "product.wxs";
				if(!File.Exists(sourceFile)) throw new ApplicationException("No wxs file could be found");
				string targetFile = fullPath + "product.wixobj";
				string outputFile = fullPath + "product.msi";

				ProcessStartInfo candleInfo = new ProcessStartInfo(WIXPATH + "candle.exe", "-nologo -out \"" + targetFile + "\" \"" + sourceFile + "\"");
				candleInfo.UseShellExecute = false;
				candleInfo.RedirectStandardOutput = true;
				Process candle = Process.Start(candleInfo);
				//candle.Start();
				candle.WaitForExit();
				Console.Write(candle.StandardOutput.ReadToEnd());

				ProcessStartInfo lightInfo = new ProcessStartInfo(WIXPATH + "light.exe", "-nologo -out \"" + outputFile + "\" \"" + targetFile + "\"");
				lightInfo.UseShellExecute = false;
				lightInfo.RedirectStandardOutput = true;
				Process light = Process.Start(lightInfo);
				//light.Start();
				light.WaitForExit();
				Console.Write(light.StandardOutput.ReadToEnd());

/*				Preprocessor preprocessor = new Preprocessor();
				preprocessor.CurrentPlatform = Platform.X86;

				XmlDocument sourceDocument = preprocessor.Process(Path.GetFullPath(sourceFile), new System.Collections.Hashtable());

				Compiler compiler = new Compiler();
				compiler.CurrentPlatform = Platform.X86;

				Intermediate intermediate = compiler.Compile(sourceDocument);
				if(intermediate != null) intermediate.Save(targetFile);

				Microsoft.Tools.WindowsInstallerXml.Tools.Light.Main(new string[] { targetFile });*/

			} catch(ApplicationException e) {
				Console.WriteLine("Error: " + e.Message);
			}

		}
	}
}
