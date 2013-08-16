using System.Globalization;

namespace XsdIncludeResolver.Extensions
{
	public static class PathExtensions
	{
		public static bool IsUrl(this string uri)
		{
			return uri.StartsWith("http", true, CultureInfo.InvariantCulture);
		}
	}
}