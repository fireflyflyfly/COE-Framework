using Atata;
using COE.Examples.Models;
using COE.Examples.Components;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = UsersPage;

    /// <summary>
    /// Users Page PageObject class
    /// </summary>
    [VerifyH1]
    public class UsersPage : Page<_>
    {
        /// <summary>
        ///   Creates new .
        /// </summary>
        /// <value>
        ///   The new.
        /// </value>
        public Button<UserEditWindow, _> New { get; private set; }

        /// <summary>
        ///   Gets the users.
        /// </summary>
        /// <value>
        ///   The users.
        /// </value>
        public Table<UserTableRow, _> Users { get; private set; }

        /// <summary>
        /// </summary>
        public class UserTableRow : TableRow<_>
        {
            /// <summary>
            ///   Gets the first name.
            /// </summary>
            /// <value>
            ///   The first name.
            /// </value>
            public Text<_> FirstName { get; private set; }

            /// <summary>
            ///   Gets the last name.
            /// </summary>
            /// <value>
            ///   The last name.
            /// </value>
            public Text<_> LastName { get; private set; }

            /// <summary>
            ///   Gets the email.
            /// </summary>
            /// <value>
            ///   The email.
            /// </value>
            public Text<_> Email { get; private set; }

            /// <summary>
            ///   Gets the office.
            /// </summary>
            /// <value>
            ///   The office.
            /// </value>
            public Content<Office, _> Office { get; private set; }

            /// <summary>
            ///   Gets the view.
            /// </summary>
            /// <value>
            ///   The view.
            /// </value>
            public Link<UserDetailsPage, _> View { get; private set; }

            /// <summary>
            ///   Gets the edit.
            /// </summary>
            /// <value>
            ///   The edit.
            /// </value>
            public Button<UserEditWindow, _> Edit { get; private set; }

            /// <summary>
            ///   Gets the delete.
            /// </summary>
            /// <value>
            ///   The delete.
            /// </value>
            [CloseConfirmBox]
            public Button<_> Delete { get; private set; }
        }
    }
}
