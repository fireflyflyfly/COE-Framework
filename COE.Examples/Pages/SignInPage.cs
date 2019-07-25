using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = SignInPage;

    /// <summary>
    /// Sign In Element PageObject class
    /// </summary>
    [Url("#/signin")]
    [VerifyH1]
    public class SignInPage : Page<_>
    {

        /// <summary>
        ///   Gets the email.
        /// </summary>
        /// <value>
        ///   The email.
        /// </value>
        public TextInput<_> Email { get; private set; }

        /// <summary>
        ///   Gets the password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        public PasswordInput<_> Password { get; private set; }

        /// <summary>
        ///   Gets the sign in.
        /// </summary>
        /// <value>
        ///   The sign in.
        /// </value>
        public Button<UsersPage, _> SignIn { get; private set; }
    }
}
