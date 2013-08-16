using FluentAssert;

using NUnit.Framework;

using XsdIncludeResolver.Extensions;

namespace XsdIncludeResolverTests.Extensions
{
	public class PathExtensionsTests
	{
		[TestFixture]
		public class When_asked_if_a_path_is_a_url
		{
			[Test]
			public void Given_a_full_file_path__should_return_false()
			{
				const string url = @"C:\temp";
				url.IsUrl().ShouldBeFalse();
			}

			[Test]
			public void Given_a_relative_file_path__should_return_false()
			{
				const string url = @"../temp";
				url.IsUrl().ShouldBeFalse();
			}

			[Test]
			public void Given_a_url_starting_with__HTTPS__should_return_true()
			{
				const string url = "HTTPS://www.google.com";
				url.IsUrl().ShouldBeTrue();
			}

			[Test]
			public void Given_a_url_starting_with__HTTP__should_return_true()
			{
				const string url = "HTTP://www.google.com";
				url.IsUrl().ShouldBeTrue();
			}

			[Test]
			public void Given_a_url_starting_with__http__should_return_true()
			{
				const string url = "http://www.google.com";
				url.IsUrl().ShouldBeTrue();
			}

			[Test]
			public void Given_a_url_starting_with__https__should_return_true()
			{
				const string url = "https://www.google.com";
				url.IsUrl().ShouldBeTrue();
			}
		}
	}
}