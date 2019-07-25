using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = PlansPage;

    /// <summary>
    /// Plans Page PageObject class
    /// </summary>
    [Url("#/plans")]
    public class PlansPage : Page<_>
    {
        /// <summary>
        ///   Gets the header.
        /// </summary>
        /// <value>
        ///   The header.
        /// </value>
        public H1<_> Header { get; private set; }

        /// <summary>
        ///   Gets the plan items.
        /// </summary>
        /// <value>
        ///   The plan items.
        /// </value>
        public ControlList<PlanItem, _> PlanItems { get; private set; }

        /// <summary>
        /// </summary>
        [ControlDefinition("div", ContainingClass = "plan-item", ComponentTypeName = "plan item")]
        public class PlanItem : Control<_>
        {
            /// <summary>
            ///   Gets the title.
            /// </summary>
            /// <value>
            ///   The title.
            /// </value>
            public H3<_> Title { get; private set; }

            /// <summary>
            ///   Gets the price.
            /// </summary>
            /// <value>
            ///   The price.
            /// </value>
            [FindByClass]
            [Culture ("en-US")]
            public Currency<_> Price { get; private set; }

            /// <summary>
            ///   Gets the number of projects.
            /// </summary>
            /// <value>
            ///   The number of projects.
            /// </value>
            [FindByClass("projects-num")]
            public Number<_> NumberOfProjects { get; private set; }

            /// <summary>
            ///   Gets the features.
            /// </summary>
            /// <value>
            ///   The features.
            /// </value>
            public UnorderedList<Text<_>, _> Features { get; private set; }
        }
    }
}
