using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Microsoft.Tools.WindowsInstallerXml;
using System.Xml;
using System.Diagnostics;
using System.Configuration;

namespace Builder {
	class Program {

		private static string runBatFile(string filename) {
			ProcessStartInfo info = new ProcessStartInfo(filename);
			info.WorkingDirectory = (new FileInfo(filename)).Directory.FullName;
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			StringBuilder builder = new StringBuilder();
			using(Process process = Process.Start(info)) {
				builder.Append(process.StandardOutput.ReadToEnd());
				process.WaitForExit();
				builder.Append(process.StandardOutput.ReadToEnd());
			}
			return builder.ToString();
		}

		static void Main(string[] args) {

			try {

				string WIXPATH = ConfigurationManager.AppSettings["WiXPath"];
				string SVNPATH = ConfigurationManager.AppSettings["SVNPath"];
				
				if(args.Length < 1) throw new ApplicationException("You should specify project name first");

				string path = args[0];
				if(!Directory.Exists(path)) throw new ApplicationException("Directory doesn't exists");
				string fullPath = new DirectoryInfo(path).FullName;
				fullPath += Path.DirectorySeparatorChar;

				string target = "release";
				string targetId = "1";
				if(args.Length > 1) {
					switch(args[1].ToLower()) {
						case "release":
							target = "release";
							targetId = "1";
							break;
						case "debug":
							target = "debug";
							targetId = "2";
							break;
						default:
							throw new ApplicationException("Wrong target");
					}
				}

				string sourceFile = fullPath + "product.wxs";
				if(!File.Exists(sourceFile)) throw new ApplicationException("No wxs file could be found");
				string targetFile = fullPath + "product.wixobj";
				string wixPdbFile = fullPath + "product-" + target + ".wixpdb";
				string outputFile = fullPath + "product-" + target + ".msi";
				string prebuildCommands = fullPath + "prebuild.bat";
				string postbuildCommands = fullPath + "postbuild.bat";
				string buildNumberFile = fullPath + "build.txt";

				if(File.Exists(prebuildCommands)) {
					Console.WriteLine(">prebuild");
					Console.WriteLine(runBatFile(prebuildCommands));
					Console.WriteLine("<prebuild");
				}

				if(!File.Exists(buildNumberFile)) {
					using(FileStream stream = File.Create(buildNumberFile)) {
						using(StreamWriter writer = new StreamWriter(stream)) {
							writer.Write("0");
						}
					}
				}
				int buildNumber;
				using(StreamReader reader = new StreamReader(buildNumberFile)) {
					buildNumber = int.Parse(reader.ReadToEnd());
				}
				buildNumber++; //NO CONCURRENCY HERE
				using(StreamWriter writer = new StreamWriter(buildNumberFile)) {
					writer.Write(buildNumber);
				}

				int revNumber;
				ProcessStartInfo svnInfo = new ProcessStartInfo(SVNPATH + "svn");
				svnInfo.WorkingDirectory = (new DirectoryInfo(".")).Parent.FullName;
				svnInfo.UseShellExecute = false;
				svnInfo.RedirectStandardOutput = true;
				svnInfo.Arguments = "up --depth=empty";
				using(Process svn = Process.Start(svnInfo)) {
					svn.WaitForExit();
				}
				svnInfo.Arguments = "info --xml";
				using(Process svn = Process.Start(svnInfo)) {
					svn.WaitForExit();
					XmlDocument document = new XmlDocument();
					document.Load(svn.StandardOutput);
					revNumber = int.Parse(document.GetElementsByTagName("entry")[0].Attributes["revision"].Value);
				}

				Console.WriteLine("Version number: 1." + revNumber + "." + buildNumber + ".0");

				using(TempFile tempFile = new TempFile()) {

					string wxsData;
					using(StreamReader sourceReader = new StreamReader(sourceFile)) {
						wxsData = sourceReader.ReadToEnd();
					}
					wxsData = wxsData.Replace("{rev}", revNumber.ToString()).Replace("{build}", buildNumber.ToString()).Replace("{target}", target).Replace("{targetId}", targetId);

					using(StreamWriter tempWriter = tempFile.getWriter()) {
						tempWriter.Write(wxsData);
					}

					ProcessStartInfo candleInfo = new ProcessStartInfo(WIXPATH + "candle.exe", "-nologo -arch x64 -out \"" + targetFile + "\" \"" + tempFile.fileName + "\"");
					candleInfo.UseShellExecute = false;
					candleInfo.RedirectStandardOutput = true;
					using(Process candle = Process.Start(candleInfo)) {
						//candle.Start();
						candle.WaitForExit();
						Console.Write(candle.StandardOutput.ReadToEnd());
					}
				}

				ProcessStartInfo lightInfo = new ProcessStartInfo(WIXPATH + "light.exe", "-nologo -out \"" + outputFile + "\" \"" + targetFile + "\"");
				lightInfo.UseShellExecute = false;
				lightInfo.RedirectStandardOutput = true;
				using(Process light = Process.Start(lightInfo)) {
					//light.Start();
					light.WaitForExit();
					Console.Write(light.StandardOutput.ReadToEnd());
				}

				File.Delete(targetFile);
				if(File.Exists(wixPdbFile)) File.Delete(wixPdbFile);

				if(File.Exists(postbuildCommands)) Console.WriteLine(runBatFile(postbuildCommands));

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
