using Atata;

/// <summary>
/// Components package for Examples of Testing automation 
/// </summary>
namespace COE.Examples.Components
{
    /// <summary>
    /// Confirmation box in JQuery component class
    /// </summary>
    /// <typeparam name="TOwner">
    ///   The type of the owner.
    /// </typeparam>
    [PageObjectDefinition("div", ContainingClass = "jconfirm-box", ComponentTypeName = "confirm box")]
    [WindowTitleElementDefinition("span", ContainingClass = "jconfirm-title")]
    public class JQueryConfirmBox<TOwner> : PopupWindow<TOwner>
        where TOwner : JQueryConfirmBox<TOwner>
    {
    }
}
