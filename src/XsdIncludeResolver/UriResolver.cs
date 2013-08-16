using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using XsdIncludeResolver.Extensions;

namespace XsdIncludeResolver
{
	public class UriResolver
	{
		private readonly IWebService _webService;

		public UriResolver(IWebService webService)
		{
			_webService = webService;
		}

		private bool FetchAndSaveXsd(XsdIncludeUri reference)
		{
			string content;
			try
			{
				content = _webService.ReadTextFromUrl(reference.Uri.AbsoluteUri);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Caught exception fetching: " + reference.Uri + " -- " + e.Message);
				return false;
			}
			var fullLocalPath = Path.GetFullPath(reference.LocalFileName);
			var directoryInfo = new FileInfo(fullLocalPath).Directory;
			if (directoryInfo != null)
			{
				directoryInfo.Create();
				File.WriteAllText(fullLocalPath, content);
				return true;
			}
			Console.Error.WriteLine("Could not create directory for: "+reference.LocalFileName);
			return false;
		}

		private static XsdIncludeUri GetSchemaLocation(string importTag, XsdIncludeUri parent)
		{
			var uri = importTag.GetUri(parent);
			if (uri == null)
			{
				return null;
			}
			var reference = new XsdIncludeUri(uri);
			return reference;
		}

		private XsdIncludeUri Resolve(XsdIncludeUri uri)
		{
			var localFileName = uri.LocalFileName;
			if (File.Exists(localFileName))
			{
				return uri;
			}
			if (!uri.IsUrl)
			{
				Console.Error.WriteLine("can't find: " + localFileName);
				return null;
			}
			if (File.Exists(uri.LocalFileName))
			{
				return uri;
			}
			if (FetchAndSaveXsd(uri))
			{
				return uri;
			}
			return null;
		}

		private Tuple<bool, IList<XsdIncludeUri>> ResolveImports(XsdIncludeUri uri)
		{
			var success = true;
			Console.WriteLine(uri.IsUrl ? uri.Uri.AbsoluteUri : uri.LocalFileName);
			var itsImports = File.ReadLines(uri.LocalFileName).Where(x => x.Contains("<xsd:import"));
			var toReturn = new List<XsdIncludeUri>();
			foreach (var import in itsImports)
			{
				var reference = GetSchemaLocation(import, uri);
				if (reference == null)
				{
					continue;
				}
				var resolved = Resolve(reference);
				if (resolved == null)
				{
					success = false;
					continue;
				}
				toReturn.Add(resolved);
			}
			return new Tuple<bool, IList<XsdIncludeUri>>(success, toReturn);
		}

		public Tuple<bool, IEnumerable<string>> ResolveRecursive(Queue<XsdIncludeUri> uris)
		{
			var success = true;
			var knownXsdPaths = new HashSet<string>();
			while (uris.Count > 0)
			{
				var fileReference = uris.Dequeue();
				if (!knownXsdPaths.Add(fileReference.LocalFileName))
				{
					continue;
				}
				var imports = ResolveImports(fileReference);
				success &= imports.Item1;
				foreach (var import in imports.Item2)
				{
					uris.Enqueue(import);
				}
			}
			return new Tuple<bool, IEnumerable<string>>(success, knownXsdPaths);
		}
	}
}