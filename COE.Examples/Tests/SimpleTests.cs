using Atata;
using NUnit.Framework;
using COE.Core.Helpers;
using COE.Examples.Pages;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    using _ = SimplePage;
    /// <summary>
    /// Tests for Home page basic elements
    /// </summary>
    public class SimpleTests : UITestFixture
    {

        /// <summary>
        ///   Test custom DDL element
        /// </summary>
        [Test]
        public void CustomDDLTest()
        {
            _ hp = Go.To<_>()
            .button.Set("CSS")
            .button.Content.Should.Equal("CSS");
        }
    }
}
