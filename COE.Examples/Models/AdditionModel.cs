using COE.Core.Helpers;

/// <summary>
/// Models package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Models
{
    /// <summary>
    /// Additon Model class used in calculation page
    /// </summary>
    public class AdditionModel
    {
        /// <summary>
        ///   Gets or sets the value1.
        /// </summary>
        /// <value>
        ///   The value1.
        /// </value>
        public int Value1 { get; set; }

        /// <summary>
        ///   Gets or sets the value2.
        /// </summary>
        /// <value>
        ///   The value2.
        /// </value>
        public int Value2 { get; set; }

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
