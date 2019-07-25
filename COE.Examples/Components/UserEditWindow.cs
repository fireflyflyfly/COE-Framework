using Atata;
using Atata.Bootstrap;
using COE.Examples.Pages;
using COE.Examples.Models;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    using _ = UserEditWindow;

    /// <summary>
    /// User Edit form class
    /// </summary>
    public class UserEditWindow : BSModal<_>
    {
        /// <summary>
        ///   Gets the general.
        /// </summary>
        /// <value>
        ///   The general.
        /// </value>
        [FindById]
        public GeneralTabPane General { get; private set; }

        /// <summary>
        ///   Gets the additional.
        /// </summary>
        /// <value>
        ///   The additional.
        /// </value>
        [FindById]
        public AdditionalTabPane Additional { get; private set; }

        /// <summary>
        ///   Gets the save.
        /// </summary>
        /// <value>
        ///   The save.
        /// </value>
        [Term("Save", "Create")]
        public Button<UsersPage, _> Save { get; private set; }

        /// <summary>
        /// </summary>
        public class GeneralTabPane : BSTabPane<_>
        {
            /// <summary>
            ///   Gets the first name.
            /// </summary>
            /// <value>
            ///   The first name.
            /// </value>
            public TextInput<_> FirstName { get; private set; }

            /// <summary>
            ///   Gets the last name.
            /// </summary>
            /// <value>
            ///   The last name.
            /// </value>
            public TextInput<_> LastName { get; private set; }

            /// <summary>
            ///   Gets the email.
            /// </summary>
            /// <value>
            ///   The email.
            /// </value>
            [RandomizeStringSettings("{0}@mail.com")]
            public TextInput<_> Email { get; private set; }

            /// <summary>
            ///   Gets the office.
            /// </summary>
            /// <value>
            ///   The office.
            /// </value>
            public Select<Office?, _> Office { get; private set; }

            /// <summary>
            ///   Gets the gender.
            /// </summary>
            /// <value>
            ///   The gender.
            /// </value>
            [FindByXPath("*[contains(text(),'Male')]/..//input")]
            public RadioButtonList<Gender?, _> Gender { get; private set; }
        }

        /// <summary>
        /// </summary>
        public class AdditionalTabPane : BSTabPane<_>
        {
            /// <summary>
            ///   Gets the birthday.
            /// </summary>
            /// <value>
            ///   The birthday.
            /// </value>
            public DateInput<_> Birthday { get; private set; }

            /// <summary>
            ///   Gets the notes.
            /// </summary>
            /// <value>
            ///   The notes.
            /// </value>
            public TextArea<_> Notes { get; private set; }
        }
    }
}
