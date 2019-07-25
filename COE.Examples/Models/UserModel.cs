using COE.Core.Helpers;

/// <summary>
/// Models package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Models
{
    /// <summary>
    /// User Model class
    /// </summary>
    public class UserModel
    {
        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value>
        ///   The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value>
        ///   The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value>
        ///   The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the office.
        /// </summary>
        /// <value>
        ///   The office.
        /// </value>
        public Office Office { get; set; }

        /// <summary>
        ///   Gets or sets the gender.
        /// </summary>
        /// <value>
        ///   The gender.
        /// </value>
        public Gender Gender { get; set; }

        /// <summary>
        ///   Converts to string.
        /// </summary>
        /// <returns>
        ///   A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return TestParametersFormatter.Format(this);
        }
    }
}
