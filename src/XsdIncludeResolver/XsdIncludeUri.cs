using System;

namespace XsdIncludeResolver
{
	public class XsdIncludeUri
	{
		public const string ErrorExpectedAFullFilePathOrUrl = "Expected a full file path or URL.";

		public XsdIncludeUri(string uri)
			:this(new Uri(uri))
		{
		}

		public XsdIncludeUri(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException(ErrorExpectedAFullFilePathOrUrl, "uri");
			}
			Uri = uri;
		}

		public string LocalFileName
		{
			get
			{
				var localFileName = Uri.IsFile ? Uri.UnescapeDataString(Uri.AbsolutePath) : Uri.AbsoluteUri;
				if (!Uri.IsFile)
				{
					var endOfProtocol = localFileName.IndexOf("://");
					localFileName = localFileName.Substring(endOfProtocol);
				}
				localFileName = localFileName.TrimStart(new[] { '.', '/', ':' });
				localFileName = localFileName.Replace('/', '\\');
				return localFileName;
			}
		}

		public Uri Uri { get; private set; }

		public bool IsUrl
		{
			get
			{
				return !Uri.IsFile;
			}
		}
	}
}