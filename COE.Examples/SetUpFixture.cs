using Atata;
using NUnit.Framework;
using COE.Core;

/// <summary>
/// Examples of Testing automation package
/// </summary>
namespace COE.Examples
{
    /// <summary>
    /// Class for setting up nunit test fixture
    /// </summary>
    class SetUpFixture : SetUpFixtureBase
    {
        /// <summary>
        ///   Examples project global setup.
        ///   Loags Project specific JSON Config into Atata configration
        /// </summary>
        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            AtataContext.GlobalConfiguration.ApplyJsonConfig<AppConfig>();
        }
    }
}
