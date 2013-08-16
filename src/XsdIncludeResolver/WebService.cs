using System.IO;
using System.Net;
using System.Text;

namespace XsdIncludeResolver
{
	public interface IWebService
	{
		string ReadTextFromUrl(string url);
	}

	public class WebService : IWebService
	{
		public string ReadTextFromUrl(string url)
		{
			// http://stackoverflow.com/questions/4510212/how-i-can-get-web-pages-content-and-save-it-into-the-string-variable
			using (var client = new WebClient())
			using (var stream = client.OpenRead(url))
			using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
			{
				return textReader.ReadToEnd();
			}
		}
	}
}