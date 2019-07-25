using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = SimplePage;

    /// <summary>
    /// Home Page PageObject class
    /// </summary>
    public class SimplePage : Page<_>
    {
        /// <summary>
        ///   Gets the sign in.
        /// </summary>
        /// <value>
        ///   The sign in.
        /// </value>
        [FindById("dropdownMenuButton")]
        public CustomDDL<_> button { get; private set; }
    }
}
