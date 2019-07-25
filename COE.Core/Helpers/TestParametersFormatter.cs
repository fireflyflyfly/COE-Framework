using System.Linq;

/// <summary>
/// Utilites, tools and helpers package for Core of Testing automation
/// </summary>
namespace COE.Core.Helpers
{
    /// <summary>
    /// Class with helpers for formatting parameters
    /// </summary>
    public static class TestParametersFormatter
    {
        /// <summary>
        ///   Formats the specified value of any type to string presentation. If it's an array then concatenate values with coma.
        /// </summary>
        /// <param name="value">
        ///   The value of any type.
        /// </param>
        /// <returns>
        ///   string representation of specified value
        /// </returns>
        public static string Format(object value)
        {
            var propertyValues = value.GetType().GetProperties().Select(x => x.GetValue(value));
            var propertyValueStrings = propertyValues.Select(x =>
                x == null ? "null" :
                x is string ? $"\"{x}\"" :
                x);
            return string.Join(",", propertyValueStrings);
        }
    }
}
