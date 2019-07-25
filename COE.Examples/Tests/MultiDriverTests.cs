using System;
using Atata;
using COE.Core;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace COE.Examples.Tests
{
    /// <summary>
    /// Multidriver tests
    /// </summary>
    [TestFixture(DriverAliases.Chrome)]
    [TestFixture(DriverAliases.Firefox)]
    [TestFixture(DriverAliases.InternetExplorer)]
    public class MultiDriverTests : UITestFixture
    {
        private readonly string driverAlias;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDriverTests"/> class.
        /// </summary>
        /// <param name="driverAlias">The driver alias.</param>
        public MultiDriverTests(string driverAlias)
        {
            this.driverAlias = driverAlias;
            this.SkipBaseBuild = true;
        }

        /// <summary>
        /// Tests set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            General.BuildContext().
                UseInternetExplorer().
                UseDriver(driverAlias).
                UseTestName(() => $"[{driverAlias}]{TestContext.CurrentContext.Test.Name}").
                Build();
        }

        /// <summary>
        /// Tests tear down.
        /// </summary>
        [TearDown]
        public void MultiDriverTearDown()
        {
            AtataContext.Current?.CleanUp();
        }

        /// <summary>
        /// Tests. Simple verifications of driver type and alias.
        /// Executed on three different drivers
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        [TestCase(4)]
        [TestCase(8)]
        public void MultiDriver_WithParameter(int parameter)
        {
            AtataContext.Current.Log.Info($"Driver alias: {driverAlias}");
            AtataContext.Current.Log.Info($"Parameter value: {parameter}");

            AtataContext.Current.DriverAlias.Should().Be(driverAlias);
            AtataContext.Current.Driver.Should().BeOfType(GetDriverTypeByAlias(driverAlias));
        }

        /// <summary>
        /// Gets the driver type by alias.
        /// </summary>
        /// <param name="driverAlias">The driver alias.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unexpected \"{driverAlias}\" value. - driverAlias</exception>
        private static Type GetDriverTypeByAlias(string driverAlias)
        {
            switch (driverAlias)
            {
                case DriverAliases.Chrome:
                    return typeof(ChromeDriver);
                case DriverAliases.InternetExplorer:
                    return typeof(InternetExplorerDriver);
                case DriverAliases.Firefox:
                    return typeof(FirefoxDriver);
                default:
                    throw new ArgumentException($"Unexpected \"{driverAlias}\" value.", nameof(driverAlias));
            }
        }
    }
}
