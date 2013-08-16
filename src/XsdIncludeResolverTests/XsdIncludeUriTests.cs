using System;

using FluentAssert;

using NUnit.Framework;

using XsdIncludeResolver;

namespace XsdIncludeResolverTests
{
	public class XsdIncludeUriTests
	{
		[TestFixture]
		public class When_asked_for_the_local_file_name
		{
			[Test]
			public void Given_the_absolute_uri__c_COLON_BACKSLASH_constraint_BACKSLASH_foo_DOT_xsd__should_return__c_COLON_BACKSLASH_constraint_BACKSLASH_foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri(new Uri(@"c:\constraint\foo.xsd", UriKind.Absolute));
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo(@"c:\constraint\foo.xsd");
			}		
			
			[Test]
			public void Given_the_absolute_uri__c_COLON_BACKSLASH_con_SPACE_straint_BACKSLASH_foo_DOT_xsd__should_return__c_COLON_BACKSLASH_constraint_BACKSLASH_foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri(new Uri(@"c:\con straint\foo.xsd", UriKind.Absolute));
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo(@"c:\con straint\foo.xsd");
			}

			[Test]
			[ExpectedException(typeof(ArgumentException))]
			public void Given_the_relative_uri__DOT_DOT_FORWARDSLASH_foo_DOT_xsd__should_return__foo_DOT_xsd()
			{
// ReSharper disable once ObjectCreationAsStatement
				new XsdIncludeUri(new Uri("../foo.xsd", UriKind.Relative));
			}

			[Test]
			[ExpectedException(typeof(UriFormatException))]
			public void Given_the_string_uri__DOT_DOT_FORWARDSLASH_foo_DOT_xsd__should_return__foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri("../foo.xsd");
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo("foo.xsd");
			}

			[Test]
			public void Given_the_string_uri__c_COLON_BACKSLASH_constraint_BACKSLASH_foo_DOT_xsd__should_return__c_COLON_BACKSLASH_constraint_BACKSLASH_foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri(@"c:\constraint\foo.xsd");
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo(@"c:\constraint\foo.xsd");
			}

			[Test]
			public void Given_the_string_uri__https_COLON_FORWARDSLASH_FORWARDSLASH_bar_DOT_com_FORWARDSLASH_foo_DOT_xsd__should_return__bar_DOT_com_BACKSLASH_foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri("https://bar.com/foo.xsd");
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo(@"bar.com\foo.xsd");
			}

			[Test]
			public void Given_the_uri__https_COLON_FORWARDSLASH_FORWARDSLASH_bar_DOT_com_FORWARDSLASH_foo_DOT_xsd__should_return__bar_DOT_com_BACKSLASH_foo_DOT_xsd()
			{
				var reference = new XsdIncludeUri(new Uri("https://bar.com/foo.xsd"));
				var localPath = reference.LocalFileName;
				localPath.ShouldBeEqualTo(@"bar.com\foo.xsd");
			}
		}

		[TestFixture]
		public class When_asked_if_the_reference_is_a_url
		{
			[Test]
			public void Given_the_reference_is_a_url__should_return_true()
			{
				var reference = new XsdIncludeUri("https://bar.com/foo.xsd");
				reference.IsUrl.ShouldBeTrue();
			}

			[Test]
			public void Given_the_reference_is_an_absolute_file_path__should_return_false()
			{
				var uri = new Uri(@"C:\temp\constraint\foo.xsd");
				var reference = new XsdIncludeUri(uri);
				reference.IsUrl.ShouldBeFalse();
			}
		}
	}
}