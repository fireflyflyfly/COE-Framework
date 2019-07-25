using Atata;
using Allure.Commons;
using NUnit.Framework;
using NUnit.Allure.Attributes;
using COE.Examples.Models;
using COE.Core.Helpers;

/// <summary>
/// Tests package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Tests
{
    /// <summary>
    /// Tests for users page
    /// </summary>
    public class UserTests : UITestFixture
    {
        /// <summary>
        /// Users the create.
        /// </summary>
        [Test]
        [AllureSuite("User")]
        public void User_Create()
        {
            string firstName, lastName, email;
            Office office = Office.NewYork;
            Gender gender = Gender.Male;

            Login(). // Returns UsersPage.
                New.ClickAndGo(). // Returns UserEditWindow.
                    ModalTitle.Should.Equal("New User").
                    General.FirstName.SetRandom(out firstName).
                    General.LastName.SetRandom(out lastName).
                    General.Email.SetRandom(out email).
                    General.Office.Set(office).
                    General.Gender.Set(gender).
                    Save.ClickAndGo(). // Returns UsersPage.
                Users.Rows[x => x.FirstName == firstName && x.LastName == lastName].View.ClickAndGo(). // Returns UserDetailsPage.
                    Header.Should.Equal($"{firstName} {lastName}").
                    Email.Should.Equal(email).
                    Office.Should.Equal(office).
                    Gender.Should.Equal(gender).
                    Birthday.Should.Not.Exist().
                    Notes.Should.Not.Exist();
        }

        /// <summary>
        /// Gets the user models.
        /// </summary>
        /// <value>
        /// The user models.
        /// </value>
        public static TestCaseData[] UserModels =>
            CsvSource.Get<UserModel>("TestData\\user-models.csv");

        /// <summary>
        /// Users the create new.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        [Test]
        [AllureSuite("User")]
        [TestCaseSource(nameof(UserModels))]
        public void User_Create_New(UserModel model)
        {
            Login().
                New.ClickAndGo().
                    General.FirstName.Set(model.FirstName).
                    General.LastName.Set(model.LastName).
                    General.Email.Set(model.Email).
                    General.Office.Set(model.Office).
                    General.Gender.Set(model.Gender).
                    Save.ClickAndGo().
                Users.Rows.Should.Contain(row =>
                    row.FirstName == model.FirstName &&
                    row.LastName == model.LastName &&
                    row.Email == model.Email &&
                    row.Office == model.Office);
        }
    }
}
