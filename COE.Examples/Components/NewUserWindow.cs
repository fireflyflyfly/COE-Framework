using Atata;
using Atata.Bootstrap;
using COE.Examples.Pages;
using COE.Examples.Models;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    using _ = NewUserWindow;

    /// <summary>
    /// New User form class
    /// </summary>
    public class NewUserWindow : BSModal<_>
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
        ///   Gets the create.
        /// </summary>
        /// <value>
        ///   The create.
        /// </value>
        public Button<UsersPage, _> Create { get; private set; }

        /// <summary>
        /// Panel with General information input fields about new user
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
            [FindByName]
            public RadioButtonList<Gender?, _> Gender { get; private set; }
        }
    }
}
