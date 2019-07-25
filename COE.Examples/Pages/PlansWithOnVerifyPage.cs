using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = PlansWithOnVerifyPage;

    /// <summary>
    /// Plans Page PageObject class with OnVerify method
    /// </summary>
    [Url("#/plans")]
    public class PlansWithOnVerifyPage : Page<_>
    {
        /// <summary>
        ///   Gets the header.
        /// </summary>
        /// <value>
        ///   The header.
        /// </value>
        public H1<_> Header { get; private set; }

        /// <summary>
        ///   Called when [verify].
        /// </summary>
        protected override void OnVerify()
        {
            base.OnVerify();

            Header.Should.Equal("Plans");
            Content.Should.Contain("Please choose your payment plan");
        }
    }
}
