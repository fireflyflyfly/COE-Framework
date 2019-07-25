using Atata;
using NUnit.Allure.Attributes;
using COE.Core;
using COE.Examples.Pages;

/// <summary>
/// Examples of Testing automation package
/// </summary>
namespace COE.Examples
{
    /// <summary>
    /// NUnit Base Test Fixture Class of Examples project .
    /// Have one additional method 
    /// </summary>
    [AllureParentSuite("UI Tests")]
    public class UITestFixture : UITestFixtureBase
    {
        /// <summary>
        /// Authorization method.
        /// Visit SignIn page. Enter login and password from config and click SignIn button, return Users list page.
        /// </summary>
        /// <returns>
        /// PageObject of Users page
        /// </returns>
        protected UsersPage Login()
        {
            return Go.To<SignInPage>()
                .Email.Set(AppConfig.Current.AccountEmail)
                .Password.Set(AppConfig.Current.AccountPassword)
                .SignIn.ClickAndGo();
        }
    }
}
