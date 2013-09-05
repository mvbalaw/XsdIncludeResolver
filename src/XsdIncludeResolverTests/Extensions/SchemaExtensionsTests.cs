using System;

using FluentAssert;

using NUnit.Framework;

using XsdIncludeResolver;
using XsdIncludeResolver.Extensions;

namespace XsdIncludeResolverTests.Extensions
{
// ReSharper disable once ClassNeverInstantiated.Global
	public class SchemaExtensionsTests
	{
		[TestFixture]
		public class When_asked_to_get_a_schema_location
		{
			[Test]
			public void Given_an_import_tag_containing_a_full_url_and_a_non_url_parent_uri__should_return_the_import_url()
			{
				const string importTag = "http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/UBL-CommonBasicComponents-2.0.xsd";
				var parent = new XsdIncludeUri(@"C:\temp\foo.xsd");
				var result = importTag.GetUri(parent);
				result.ShouldBeEqualTo(new Uri(@"http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/UBL-CommonBasicComponents-2.0.xsd"));
			}

			[Test]
			public void Given_an_import_tag_containing_a_full_url_and_a_url_parent_uri__should_return_the_import_url()
			{
				const string importTag = "http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/UBL-CommonBasicComponents-2.0.xsd";
				var parent = new XsdIncludeUri(@"http://niem.gov/niem/niem-core/2.0/niem-core.xsd");
				var result = importTag.GetUri(parent);
				result.ShouldBeEqualTo(new Uri(@"http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/UBL-CommonBasicComponents-2.0.xsd"));
			}

			[Test]
			public void Given_an_import_tag_containing_a_relative_file_path_and_a_non_url_parent_uri__should_combine_and_return_the_parent_uri_base_path_and_import_file_path()
			{
				const string importTag = "../constraint/niem-core.xsd";
				var parent = new XsdIncludeUri(@"C:\temp\foo.xsd");
				var result = importTag.GetUri(parent);
				result.LocalPath.ShouldBeEqualTo(@"C:\constraint\niem-core.xsd");
			}

			[Test]
			public void Given_an_import_tag_containing_only_a_file_name_and_a_non_url_parent_uri__should_combine_and_return_the_parent_uri_base_path_and_import_file_name()
			{
				const string importTag = "niem-core.xsd";
				var parent = new XsdIncludeUri(@"C:\temp\foo.xsd");
				var result = importTag.GetUri(parent);
				result.LocalPath.ShouldBeEqualTo(@"C:\temp\niem-core.xsd");
			}

			[Test]
			public void Given_an_import_tag_containing_only_a_file_name_and_a_url_parent_uri__should_combine_and_return_the_parent_uri_base_path_and_import_file_path()
			{
				const string importTag = "UBL-CommonBasicComponents-2.0.xsd";
				var parent = new XsdIncludeUri(@"http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/common/UBL-CommonAggregateComponents-2.0.xsd");
				var result = importTag.GetUri(parent);
				result.ShouldBeEqualTo(new Uri(@"http://docs.oasis-open.org/ubl/cs-UBL-2.0/xsd/common/UBL-CommonBasicComponents-2.0.xsd"));
			}
		}
	}
}