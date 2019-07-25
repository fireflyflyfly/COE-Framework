using Atata;
using NUnit.Framework;
using NUnit.Allure.Attributes;
using Allure.Commons;
using COE.Examples.Pages;
using COE.Examples.Extensions;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for SignUp page
    /// </summary>
    class SignUpTests : UITestFixture
    {
        /// <summary>
        /// Signs up validation required.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        [AllureSeverity(SeverityLevel.normal)]
        public void SignUp_Validation_Required()
        {
            Go.To<SignUpPage>().
                SignUp.Click().
                ValidationMessages[x => x.FirstName].Should.EndWithIgnoringCase("field is required.").
                ValidationMessages[x => x.LastName].Should.EndWithIgnoringCase("field is required.").
                ValidationMessages[x => x.Email].Should.EndWithIgnoringCase("field is required.").
                ValidationMessages[x => x.Password].Should.EndWithIgnoringCase("field is required.").
                ValidationMessages[x => x.Agreement].Should.EndWithIgnoringCase("field is required.").
                ValidationMessages.Should.HaveCount(5);
        }

        /// <summary>
        /// Signs up validation required using extensions.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        public void SignUp_Validation_Required_UsingExtensions()
        {
            Go.To<SignUpPage>().
                SignUp.Click().
                ValidationMessages[x => x.FirstName].Should.BeRequired().
                ValidationMessages[x => x.LastName].Should.BeRequired().
                ValidationMessages[x => x.Email].Should.BeRequired().
                ValidationMessages[x => x.Password].Should.BeRequired().
                ValidationMessages[x => x.Agreement].Should.BeRequired().
                ValidationMessages.Should.HaveCount(5);
        }

        /// <summary>
        /// Signs the minimum length of up validation.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        public void SignUp_Validation_MinLength()
        {
            Go.To<SignUpPage>().
                FirstName.Set("a").
                LastName.Set("a").
                Password.Set("a").
                SignUp.Click().
                ValidationMessages[x => x.FirstName].Should.EndWithIgnoringCase("field must be at least 2 characters.").
                ValidationMessages[x => x.LastName].Should.EndWithIgnoringCase("field must be at least 2 characters.").
                ValidationMessages[x => x.Password].Should.EndWithIgnoringCase("field must be at least 6 characters.");
        }

        /// <summary>
        /// Signs up validation minimum length using extensions.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        public void SignUp_Validation_MinLength_UsingExtensions()
        {
            Go.To<SignUpPage>().
                FirstName.Set("a").
                LastName.Set("a").
                Password.Set("a").
                SignUp.Click().
                ValidationMessages[x => x.FirstName].Should.HaveMinLength(2).
                ValidationMessages[x => x.LastName].Should.HaveMinLength(2).
                ValidationMessages[x => x.Password].Should.HaveMinLength(6);
        }

        /// <summary>
        /// Signs up validation incorrect email.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        public void SignUp_Validation_IncorrectEmail()
        {
            Go.To<SignUpPage>().
                Email.Set("some@email").
                SignUp.Click().
                ValidationMessages[x => x.Email].Should.Equal("The email field must be a valid email.").
                Email.Append(".com").
                SignUp.Click().
                ValidationMessages[x => x.Email].Should.Not.Exist();
        }

        /// <summary>
        /// Signs up validation incorrect email using extensions.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        [AllureSubSuite("SignUp")]
        public void SignUp_Validation_IncorrectEmail_UsingExtensions()
        {
            Go.To<SignUpPage>().
                Email.Set("some@email").
                SignUp.Click().
                ValidationMessages[x => x.Email].Should.HaveIncorrectFormat().
                Email.Append(".com").
                SignUp.Click().
                ValidationMessages[x => x.Email].Should.Not.Exist();
        }
    }
}
