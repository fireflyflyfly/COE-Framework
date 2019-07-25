using System;
using Atata;
using COE.Examples.Models;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = UserDetailsPage;

    /// <summary>
    /// User Details Page PageObject class
    /// </summary>
    public class UserDetailsPage : Page<_>
    {
        /// <summary>
        ///   Gets the header.
        /// </summary>
        /// <value>
        ///   The header.
        /// </value>
        [FindFirst]
        public H1<_> Header { get; private set; }

        /// <summary>
        ///   Gets the email.
        /// </summary>
        /// <value>
        ///   The email.
        /// </value>
        [FindByDescriptionTerm]
        public Text<_> Email { get; private set; }

        /// <summary>
        ///   Gets the office.
        /// </summary>
        /// <value>
        ///   The office.
        /// </value>
        [FindByDescriptionTerm]
        public Content<Office, _> Office { get; private set; }

        /// <summary>
        ///   Gets the gender.
        /// </summary>
        /// <value>
        ///   The gender.
        /// </value>
        [FindByDescriptionTerm]
        public Content<Gender, _> Gender { get; private set; }

        /// <summary>
        ///   Gets the birthday.
        /// </summary>
        /// <value>
        ///   The birthday.
        /// </value>
        [FindByDescriptionTerm]
        public Content<DateTime?, _> Birthday { get; private set; }

        /// <summary>
        ///   Gets the notes.
        /// </summary>
        /// <value>
        ///   The notes.
        /// </value>
        [FindByDescriptionTerm]
        public Text<_> Notes { get; private set; }
    }
}
