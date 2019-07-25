using Allure.Commons;
using Atata;
using Atata.Configuration.Json;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.IO;

/// <summary>
/// Core of Testing automation package 
/// </summary>
namespace COE.Core
{
    /// <summary>
    /// Base Class for setting up nunit test fixture
    /// </summary>
    [SetUpFixture]
    public class SetUpFixtureBase
    {
        /// <summary>
        ///   One Time Setup method for Test Fixture. Creates Atata Context builder, configure it and build
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            AtataContext.GlobalConfiguration.ApplyJsonConfig<AppData>();
            AllureExtensions.WrapSetUpTearDownParams(() => { AllureLifecycle.Instance.CleanupResultDirectory(); });
            if (General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver)
            {
                General.BuildContext().Build();
            }
        }

        /// <summary>
        ///   One Time Tear down method for Test Fixture. Clean up current Atata Context 
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            if (General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver)
            {
                AtataContext.Current?.CleanUp();
            }
        }

    }
}
