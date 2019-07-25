using Atata;
using NUnit.Framework;
using NUnit.Allure.Attributes;
using COE.Examples.Models;
using COE.Core.Helpers;
using COE.Examples.Pages;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for Calculation page
    /// </summary>
    public class CalculationTests : UITestFixture
    {
        /// <summary>
        ///   Gets the addition models.
        /// </summary>
        /// <value>
        ///   The addition models.
        /// </value>
        public static TestCaseData[] AdditionModels =>
            CsvSource.Get<AdditionModel>("TestData\\addition-models.csv", expectedResultType: typeof(int));

        /// <summary>
        ///   Calculations the addition.
        ///   Demonstrates usage of TestCaseSource attribute, using CSV file as data source
        ///   
        /// </summary>
        /// <param name="model">
        ///   The model.
        /// </param>
        /// <returns></returns>
        [Test]
        [AllureTag("TC-1")]
        [AllureSuite("Calculation")]
        [TestCaseSource(nameof(AdditionModels))]
        public int Calculation_Addition(AdditionModel model)
        {
            return Go.To<CalculationsPage>().
                AdditionValue1.Set(model.Value1).
                AdditionValue2.Set(model.Value2).
                AdditionResult.Value;
        }
    }
}
