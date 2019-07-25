using Atata;
using Atata.Bootstrap;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    /// <summary>
    /// Class component for alert confirmation of delete action with two buttons: delete and cancel
    /// </summary>
    /// <typeparam name="TNavigateTo">
    ///   Defines PageObject to navigate to after confirming delete action
    /// </typeparam>
    [Name("Deletion Confirmation")]
    [WindowTitle("Confirmation")]
    public class DeletionConfirmationBSModal<TNavigateTo> : BSModal<DeletionConfirmationBSModal<TNavigateTo>>
        where TNavigateTo : PageObject<TNavigateTo>
    {
        /// <summary>
        ///   Property for delete button.
        /// </summary>
        /// <value>
        ///   The delete button.
        /// </value>
        public ButtonDelegate<TNavigateTo, DeletionConfirmationBSModal<TNavigateTo>> Delete { get; private set; }

        /// <summary>
        ///   Property for cancel button.
        /// </summary>
        /// <value>
        ///   The cancel button.
        /// </value>
        public ButtonDelegate<TNavigateTo, DeletionConfirmationBSModal<TNavigateTo>> Cancel { get; private set; }
    }
}
