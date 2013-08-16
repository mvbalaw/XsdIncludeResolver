using System;

namespace XsdIncludeResolver.Extensions
{
	public static class SchemaExtensions
	{
		private static Uri CreateFileUri(string location, string parentPath)
		{
			var lastSlash = 1 + parentPath.LastIndexOf('\\');
			var baseParentUri = parentPath.Substring(0, lastSlash);
			Uri result;
			if (!Uri.TryCreate(new Uri(baseParentUri), location, out result))
			{
				Console.Error.WriteLine("Could not create uri from:" + baseParentUri + " AND " + location);
				return null;
			}
			return result;
		}

		private static Uri CreateUrlUri(string location, string parentPath)
		{
			if (!location.IsUrl())
			{
				var lastSlash = 1+parentPath.LastIndexOf('/');
				var baseParentUri = parentPath.Substring(0, lastSlash);
				Uri result;
				if (!Uri.TryCreate(new Uri(baseParentUri), location, out result))
				{
					Console.Error.WriteLine("Could not create uri from:" + baseParentUri + " AND " + location);
					return null;
				}
				return result;
			}
			return new Uri(location);
		}

		public static Uri GetUri(this string importTag, XsdIncludeUri parent)
		{
			var start = importTag.IndexOf("schemaLocation=\"") + "schemaLocation=\"".Length;
			var end = importTag.IndexOf('"', start);
			var location = importTag.Substring(start, end - start);
			return parent.IsUrl ? CreateUrlUri(location, parent.Uri.AbsoluteUri) : CreateFileUri(location, parent.Uri.LocalPath);
		}
	}
}