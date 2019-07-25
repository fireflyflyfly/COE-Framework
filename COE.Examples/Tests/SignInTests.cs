using Atata;
using NUnit.Framework;
using NUnit.Allure.Attributes;
using Allure.Commons;
using COE.Examples.Pages;
using COE.Core;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for SignIn element in the header
    /// </summary>
    class SignInTests : UITestFixture
    {
        /// <summary>
        ///  Demonstrates Sign In test.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignIn")]
        [AllureSeverity(SeverityLevel.critical)]
        public void SignIn()
        {
            Go.To<HomePage>()
                .SignIn.ClickAndGo()
                .Email.Set(AppConfig.Current.AccountEmail)
                .Password.Set(AppConfig.Current.AccountPassword)
                .SignIn.ClickAndGo()
                .Users.Should.Exist()
                .New.Should.Exist()
                ;
        }
    }
}
