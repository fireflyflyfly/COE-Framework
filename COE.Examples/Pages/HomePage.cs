using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = HomePage;

    /// <summary>
    /// Home Page PageObject class
    /// </summary>
    public class HomePage : Page<_>
    {
        /// <summary>
        ///   Gets the sign in.
        /// </summary>
        /// <value>
        ///   The sign in.
        /// </value>
        [FindById]
        public Link<SignInPage, _> SignIn { get; private set; }
    }
}
