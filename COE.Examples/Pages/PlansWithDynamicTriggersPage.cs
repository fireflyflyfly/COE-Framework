using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = PlansWithDynamicTriggersPage;

    /// <summary>
    /// Plans Page PageObject class with dynamic trigger verification
    /// </summary>
    [Url("#/plans")]
    public class PlansWithDynamicTriggersPage : Page<_>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PlansWithDynamicTriggersPage" /> class.
        /// </summary>
        public PlansWithDynamicTriggersPage()
        {
            Triggers.Add(
                new VerifyH1Attribute("Plans"),
                new VerifyContentAttribute("Please choose your payment plan"));
        }
    }
}
