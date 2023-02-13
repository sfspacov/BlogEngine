﻿using NUnit.Framework;
using BlogEngine.Core.Common.Extensions;

namespace BlogEngine.Tests.BlogEngine.Core.Common.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void StripHtmlTagsWithRegex_NullOrWhiteSpaceRawHtmlContent_ThrowArgumentNullException(string rawHtmlContent)
        {
            Assert.That(() => rawHtmlContent.StripHtmlTagsWithRegex(),
                Throws.ArgumentNullException);
        }

        [Test]
        [TestCase("<h1>Bar</h1>", "Bar")]
        [TestCase("<br> <h1>Foo</h1>", " Foo")]
        [TestCase("<p>Baz</p> <h1>Foo</h1>", "Baz Foo")]
        public void StripHtmlTagsWithRegex_ValidRawHtmlContent_StripsHtmlTagsWithRegex(string rawHtmlContent, string expectedResult)
        {
            var result = rawHtmlContent.StripHtmlTagsWithRegex();

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}