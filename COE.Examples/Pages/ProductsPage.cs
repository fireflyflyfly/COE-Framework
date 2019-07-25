using Atata;
using COE.Examples.Components;
using COE.Examples.Triggers;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Pages
{
    using _ = ProductsPage;

    /// <summary>
    /// Products Page PageObject class
    /// </summary>
    [Url("#/products")]
    public class ProductsPage : Page<_>
    {
        /// <summary>
        ///   Gets the products table control.
        /// </summary>
        /// <value>
        ///   The products table control.
        /// </value>
        public Table<ProductTableRow, _> Products { get; private set; }

        /// <summary>
        /// Class describes Table row  control specified for product row
        /// </summary>
        public class ProductTableRow : TableRow<_>
        {
            /// <summary>
            ///   Gets the name column control.
            /// </summary>
            /// <value>
            ///   The name column control.
            /// </value>
            public Text<_> Name { get; private set; }

            /// <summary>
            ///   Gets the price column control.
            /// </summary>
            /// <value>
            ///   The price column control.
            /// </value>
            public Currency<_> Price { get; private set; }

            /// <summary>
            ///   Gets the amount column control.
            /// </summary>
            /// <value>
            ///   The amount column control.
            /// </value>
            public Number<_> Amount { get; private set; }

            /// <summary>
            ///   Gets the delete button control for using js confirm.
            /// </summary>
            /// <value>
            ///   The delete button control for using js confirm.
            /// </value>
            [CloseConfirmBox]
            public ButtonDelegate<_> DeleteUsingJSConfirm { get; private set; }

            /// <summary>
            ///   Gets the delete button control for bs modal.
            /// </summary>
            /// <value>
            ///   The delete button control for bs modal.
            /// </value>
            [FindByContent("Delete Using BS Modal")]
            public ButtonDelegate<DeletionConfirmationBSModal<_>, _> DeleteUsingBSModal { get; private set; }

            /// <summary>
            ///   Executes trigger and gets the delete button control for bs modal via trigger.
            /// </summary>
            /// <value>
            ///   The delete button control for bs modal via trigger.
            /// </value>
            [FindByContent("Delete Using BS Modal")]
            [ConfirmDeletionViaBSModal]
            public ButtonDelegate<_> DeleteUsingBSModalViaTrigger { get; private set; }

            /// <summary>
            ///   Gets the delete button control for JQuery confirm.
            /// </summary>
            /// <value>
            ///   The delete  button control for JQuery confirm.
            /// </value>
            [FindByContent("Delete Using jquery-confirm")]
            public ButtonDelegate<DeletionJQueryConfirmBox<_>, _> DeleteUsingJQueryConfirm { get; private set; }

            /// <summary>
            ///   Executes trigger and get Delete button control for JQuery confirm via trigger.
            /// </summary>
            /// <value>
            ///   The delete  button control for JQuery confirm via trigger.
            /// </value>
            [FindByContent("Delete Using jquery-confirm")]
            [ConfirmDeletionViaJQueryConfirmBox]
            public ButtonDelegate<_> DeleteUsingJQueryConfirmViaTrigger { get; private set; }
        }
    }
}
