using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XsdIncludeResolver
{
	internal class Program
	{
		private const string ParametersFilePattern = @"
<xsd xmlns='http://microsoft.com/dotnet/tools/xsd/'>
	<generateClasses language='CS' namespace='{0}'>
		{1}
	</generateClasses>
</xsd>";

		private static void Main(string[] args)
		{
			if (args.Length == 0 || args[0] == "/?")
			{
				Console.WriteLine("Usage: namespace xsd1 [xsd2...xsdN]");
				return;
			}
			var @namespace = args[0];

			var xsdFileQueue = new Queue<XsdIncludeUri>();
			foreach (var uri in args.Skip(1))
			{
				xsdFileQueue.Enqueue(new XsdIncludeUri(Path.GetFullPath(uri)));
			}
			var resolver = new UriResolver(new WebService());
			var result = resolver.ResolveRecursive(xsdFileQueue);
			if (!result.Item1)
			{
				Console.Error.WriteLine("stopping due to error(s)");
				return;
			}

			WriteParametersFile(result.Item2, @namespace);
		}

		private static void WriteParametersFile(IEnumerable<string> knownXsds, string @namespace)
		{
			const string parametersFile = ".\\parameters.xml";
			var includeXml = new StringBuilder();
			foreach (var entry in knownXsds)
			{
				includeXml.AppendFormat("\t\t<schema>{0}</schema>{1}{1}", entry, Environment.NewLine);
			}
			Console.WriteLine("built parameters file: " + parametersFile);
			Console.WriteLine("use it with: ");
			Console.WriteLine("xsd.exe /c /out:[output dir] /p:" + parametersFile);
			Console.WriteLine(@"if xsd.exe is not in your path try: ""C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\xsd.exe"" ...");
			File.WriteAllText(parametersFile, String.Format(ParametersFilePattern, @namespace, includeXml));
		}
	}
}