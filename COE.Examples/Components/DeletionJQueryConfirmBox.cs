using Atata;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    /// <summary>
    /// Delete confirmation box on JQuery component class
    /// </summary>
    /// <typeparam name="TNavigateTo">
    ///   The type of the navigate to.
    /// </typeparam>
    [Name("Deletion Confirmation")]
    [WindowTitle("Confirmation")]
    public class DeletionJQueryConfirmBox<TNavigateTo> : JQueryConfirmBox<DeletionJQueryConfirmBox<TNavigateTo>>
        where TNavigateTo : PageObject<TNavigateTo>
    {
        /// <summary>
        ///   Gets the delete button delegate.
        /// </summary>
        /// <value>
        ///   The delete button delegate.
        /// </value>
        [Term(TermCase.MidSentence)]
        public ButtonDelegate<TNavigateTo, DeletionJQueryConfirmBox<TNavigateTo>> Delete { get; private set; }

        /// <summary>
        ///   Gets the cancel button delegate.
        /// </summary>
        /// <value>
        ///   The cancel button delegate.
        /// </value>
        [Term(TermCase.MidSentence)]
        public ButtonDelegate<TNavigateTo, DeletionJQueryConfirmBox<TNavigateTo>> Cancel { get; private set; }
    }
}
