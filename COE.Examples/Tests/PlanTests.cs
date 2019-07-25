using Atata;
using NUnit.Framework;
using NUnit.Allure.Attributes;
using COE.Examples.Pages;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for plan page
    /// </summary>
    class PlanTests : UITestFixture
    {
        private const string Feature1 = "Feature 1";
        private const string Feature2 = "Feature 2";
        private const string Feature3 = "Feature 3";
        private const string Feature4 = "Feature 4";
        private const string Feature5 = "Feature 5";
        private const string Feature6 = "Feature 6";

        /// <summary>
        ///   Demonstrates the page data verification in test.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void PrimaryPageDataVerification_InTest()
        {
            Go.To<PlansPage>().
                Header.Should.Equal("Plans").
                Content.Should.Contain("Please choose your payment plan");
        }

        /// <summary>
        ///   Demonstrates the page data verification on verify.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void PrimaryPageDataVerification_OnVerify()
        {
            Go.To<PlansWithOnVerifyPage>();
        }

        /// <summary>
        ///   Demonstrates the page data verification static triggers.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void PrimaryPageDataVerification_StaticTriggers()
        {
            Go.To<PlansWithStaticTriggersPage>();
        }

        /// <summary>
        ///   Demonstrates the page data verification dynamic triggers.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void PrimaryPageDataVerification_DynamicTriggers()
        {
            Go.To<PlansWithDynamicTriggersPage>();
        }

        /// <summary>
        ///   Demonstrates complexes the page data verification.
        ///   Demonstrates Currency component.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void ComplexPageDataVerification()
        {
            Go.To<PlansPage>().
                PlanItems.Count.Should.Equal(3).

                PlanItems[0].Title.Should.Equal("Basic").
                PlanItems[0].Price.Should.Equal(0).
                PlanItems[0].NumberOfProjects.Should.Equal(1).
                PlanItems[0].Features.Items.Should.EqualSequence(Feature1, Feature2).

                PlanItems[1].Title.Should.Equal("Plus").
                PlanItems[1].Price.Should.Equal(19.99m).
                PlanItems[1].NumberOfProjects.Should.Equal(3).
                PlanItems[1].Features.Items.Should.EqualSequence(Feature1, Feature2, Feature3, Feature4).

                PlanItems[2].Title.Should.Equal("Premium").
                PlanItems[2].Price.Should.Equal(49.99m).
                PlanItems[2].NumberOfProjects.Should.Equal(10).
                PlanItems[2].Features.Items.Should.EqualSequence(Feature1, Feature2, Feature3, Feature4, Feature5, Feature6);
        }

        /// <summary>
        ///   Demonstrates complexes the page data verification with Soft assertion.
        /// </summary>
        [Test]
        [AllureSuite("Plans")]
        public void SoftAssertionComplexPageDataVerification()
        {
            Go.To<PlansPage>()
                .PlanItems.Count.Should.EqualSoft(3)

                .PlanItems[0].Title.Should.EqualSoft("Basic")
                .PlanItems[0].Price.Should.EqualSoft(0)
                .PlanItems[0].NumberOfProjects.Should.EqualSoft(1)
                .PlanItems[0].Features.Items.Should.EqualSequenceSoft(Feature1, Feature2)

                .PlanItems[1].Title.Should.EqualSoft("Plus")
                .PlanItems[1].Price.Should.EqualSoft(19.99m)
                .PlanItems[1].NumberOfProjects.Should.EqualSoft(3)
                .PlanItems[1].Features.Items.Should.EqualSequenceSoft(Feature1, Feature2, Feature3, Feature4)

                .PlanItems[2].Title.Should.EqualSoft("Premium")
                .PlanItems[2].Price.Should.EqualSoft(49.99m)
                .PlanItems[2].NumberOfProjects.Should.EqualSoft(10)
                .PlanItems[2].Features.Items.Should.EqualSequenceSoft(Feature1, Feature2, Feature3, Feature4, Feature5, Feature6)
                
                .ThrowSoftAsserts();
        }

    }
}
