using Atata.Configuration.Json;
using COE.Core;

/// <summary>
/// Examples of Testing automation package
/// </summary>
namespace COE.Examples
{
    /// <summary>
    /// Application configuration class.
    /// Includes project specific properties that must be specified in Atata.json file
    /// </summary>
    public class AppConfig : AppData<AppConfig>
    {
        /// <summary>
        ///   Admin Account email property.
        /// </summary>
        /// <value>
        ///   The account email.
        /// </value>
        public string AccountEmail { get; set; }

        /// <summary>
        ///   Admin Account password property.
        /// </summary>
        /// <value>
        ///   The account password.
        /// </value>
        public string AccountPassword { get; set; }
    }
}
