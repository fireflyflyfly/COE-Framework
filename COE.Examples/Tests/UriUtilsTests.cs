using System;
using Atata;
using FluentAssertions;
using NUnit.Framework;

namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for UriUtils class
    /// </summary>
    [TestFixture]
    public class UriUtilsTests
    {
        /// <summary>
        /// Demonstrates work of UriUtils class. Try create absolute URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="isAbsolute">if set to <c>true</c> [is absolute].</param>
        [TestCase("http://something.com", true)]
        [TestCase("https://something.com", true)]
        [TestCase("ftp://something.com", true)]
        [TestCase("custom://something.com", true)]
        [TestCase("http:/something.com", false)]
        [TestCase("//something", false)]
        [TestCase("/something", false)]
        [TestCase("something", false)]
        [TestCase(null, false)]
        public void UriUtils_TryCreateAbsoluteUrl(string url, bool isAbsolute)
        {
            var isActuallyAbsolute = UriUtils.TryCreateAbsoluteUrl(url, out Uri result);
            isActuallyAbsolute.Should().Be(isAbsolute);

            if (isAbsolute)
                result.AbsoluteUri.Should().StartWith(url);
            else
                result.Should().BeNull();
        }
    }
}
