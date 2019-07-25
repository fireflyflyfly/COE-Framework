using System;
using Atata;
using OpenQA.Selenium;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TOwner">
    ///   The type of the owner.
    /// </typeparam>
    public class ValidationMessageList<TOwner> : ControlList<ValidationMessage<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        ///   Gets the <see cref="ValidationMessage{TOwner}" /> with the specified control selector.
        /// </summary>
        /// <param name="controlSelector">
        ///   The control selector.
        /// </param>
        /// <returns></returns>
        /// <value>
        ///   The <see cref="ValidationMessage{TOwner}" />.
        /// </value>
        public ValidationMessage<TOwner> this[Func<TOwner, IControl<TOwner>> controlSelector]
        {
            get { return For(controlSelector); }
        }

        /// <summary>
        ///   Fors the specified control selector.
        /// </summary>
        /// <param name="controlSelector">
        ///   The control selector.
        /// </param>
        /// <returns></returns>
        public ValidationMessage<TOwner> For(Func<TOwner, IControl<TOwner>> controlSelector)
        {
            var validationMessageDefinition = UIComponentResolver.GetControlDefinition(typeof(ValidationMessage<TOwner>));

            IControl<TOwner> boundControl = controlSelector(Component.Owner);

            PlainScopeLocator scopeLocator = new PlainScopeLocator(By.XPath("ancestor::" + validationMessageDefinition.ScopeXPath))
            {
                SearchContext = boundControl.Scope
            };

            return Component.Controls.Create<ValidationMessage<TOwner>>(boundControl.ComponentName, scopeLocator);
        }
    }
}
