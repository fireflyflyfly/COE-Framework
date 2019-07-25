using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = PlansWithStaticTriggersPage;

    /// <summary>
    /// Plans Page PageObject class with static Trigger
    /// </summary>
    [Url("#/plans")]
    [VerifyH1("Plans")]
    [VerifyContent("Please choose your payment plan")]
    public class PlansWithStaticTriggersPage : Page<_>
    {
    }
}
