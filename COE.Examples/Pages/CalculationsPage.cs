using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = CalculationsPage;

    /// <summary>
    /// Calculations Page PageObject class
    /// </summary>
    [Url("#/calculations")]
    public class CalculationsPage : Page<_>
    {
        /// <summary>
        ///   Gets the addition value1.
        /// </summary>
        /// <value>
        ///   The addition value1.
        /// </value>
        [FindById]
        public Input<int, _> AdditionValue1 { get; private set; }

        /// <summary>
        ///   Gets the addition value2.
        /// </summary>
        /// <value>
        ///   The addition value2.
        /// </value>
        [FindById]
        public Input<int, _> AdditionValue2 { get; private set; }

        /// <summary>
        ///   Gets the addition result.
        /// </summary>
        /// <value>
        ///   The addition result.
        /// </value>
        [FindById]
        public Input<int, _> AdditionResult { get; private set; }
    }
}
