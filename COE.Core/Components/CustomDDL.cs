using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Has method which allow to change value of Bootstrap-like dropdown select-boxes by desired value.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class CustomDDL<TOwner> : Clickable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public TOwner Set(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataSettingLogSection(this, value));

            this.Click();
            this.Scope.FindElement(By.XPath($"./parent::div//a[.='{value}']")).Click();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }
    }
}
