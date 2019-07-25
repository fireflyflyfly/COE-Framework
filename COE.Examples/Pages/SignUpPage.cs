using Atata;
using COE.Examples.Components;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = SignUpPage;

    /// <summary>
    /// Sign Up Page PageObject class
    /// </summary>
    [Url("#/signup")]
    public class SignUpPage : Page<_>
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
        ///   Gets the password.
        /// </summary>
        /// <value>
        ///   The password.
        /// </value>
        public PasswordInput<_> Password { get; private set; }

        /// <summary>
        ///   Gets the agreement.
        /// </summary>
        /// <value>
        ///   The agreement.
        /// </value>
        [FindByLabel("I agree to terms of service and privacy policy")]
        public CheckBox<_> Agreement { get; private set; }

        /// <summary>
        ///   Gets the sign up.
        /// </summary>
        /// <value>
        ///   The sign up.
        /// </value>
        public Button<_> SignUp { get; private set; }

        /// <summary>
        ///   Gets the validation messages.
        /// </summary>
        /// <value>
        ///   The validation messages.
        /// </value>
        public ValidationMessageList<_> ValidationMessages { get; private set; }
    }
}
