using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
	class Program
	{
		const string ParametersFilePattern = @"
<xsd xmlns='http://microsoft.com/dotnet/tools/xsd/'>
	<generateClasses language='CS' namespace='{0}'>
		{1}
	</generateClasses>
</xsd>";

		static void Main(string[] args)
		{
			var @namespace = args[0];
			var allXsdPaths = FindAllXsds(".");
			Console.WriteLine("Found "+allXsdPaths.Count+" XSDs under this location.");
			var knownXsds = new HashSet<string>();
			var xsdFileQueue = new Queue<string>();
			var error = false;
			foreach (var xsd in args.Skip(1))
			{
				xsdFileQueue.Enqueue(Path.GetFullPath(xsd));
			}
			while (xsdFileQueue.Count > 0)
			{
				var file = xsdFileQueue.Dequeue();
				if (!knownXsds.Add(file))
				{
					continue;
				}
				Console.WriteLine(file);
				var itsImports = File.ReadLines(file).Where(x => x.Contains("<xsd:import"));
				foreach (var import in itsImports)
				{
					var start = import.IndexOf("schemaLocation=\"") + "schemaLocation=\"".Length;
					var fileName = import.Substring(start);
					var end = fileName.IndexOf('"');
					fileName = fileName.Substring(0, end);
					fileName = fileName.TrimStart(new[] { '.', '/' });
					fileName = fileName.Replace('/', '\\');
					var matches = allXsdPaths.Where(x => x.EndsWith(fileName)).ToList();
					if (!matches.Any())
					{
						Console.Error.WriteLine("can't find: "+fileName);
						error = true;
						continue;
					}
					if (matches.Count > 1)
					{
						Console.Error.WriteLine("found multiple matches for: " + fileName);
						error = true;
						continue;
					}
					xsdFileQueue.Enqueue(matches.First());
				}
			}
			if (error)
			{
				Console.WriteLine("stopping due to error(s)");
				return;
			}

			const string parametersFile = ".\\parameters.xml";
			var includeXml = new StringBuilder();
			foreach (var entry in knownXsds)
			{
				includeXml.AppendFormat("\t\t<schema>{0}</schema>{1}{1}", entry, Environment.NewLine);
			}
			Console.WriteLine("built parameters file: "+parametersFile);
			Console.WriteLine("use it with: ");
			Console.WriteLine("xsd.exe /c /out:[output dir] /p:" + parametersFile);
			Console.WriteLine(@"if xsd.exe is not in your path try: ""C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe"" ...");
			File.WriteAllText(parametersFile, String.Format(ParametersFilePattern, @namespace, includeXml));
		}

		private static HashSet<string> FindAllXsds(string directoryName)
		{
			var path = Path.GetFullPath(directoryName);
			return new HashSet<string>(Directory.GetFiles(path, "*.xsd", SearchOption.AllDirectories).Where(x=>!x.Contains("original")).Where(x=>!x.Contains("Subset")));
		}
	}
}
