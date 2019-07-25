using Atata;
using COE.Examples.Components;

/// <summary>
/// Triggers package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Triggers
{
    /// <summary>
    /// Class describes custom Trigger Attribute 
    /// It clicks on control then executes action wich clicks on Delete button on popup window of JQuery Confirm box.
    /// </summary>
    public class ConfirmDeletionViaJQueryConfirmBoxAttribute : TriggerAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmDeletionViaJQueryConfirmBoxAttribute"/> class.
        /// </summary>
        /// <param name="on">The on.</param>
        /// <param name="priority">The priority.</param>
        public ConfirmDeletionViaJQueryConfirmBoxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>Executes the specified trigger action.</summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="context">The trigger context.</param>
        protected override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            Go.To<DeletionJQueryConfirmBox<TOwner>>(temporarily: true).
                Delete();
        }
    }
}
