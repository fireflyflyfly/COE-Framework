using COE.Core.Helpers;
using Atata;

namespace COE.Core.Pages
{
    /// <summary>
    /// Base PageObject class
    /// </summary>
    public abstract class BasePage<TOwner> : Page<TOwner> where TOwner : Page<TOwner>
    {
        public PageHelper<TOwner> _pageHelper = new PageHelper<TOwner>();

        public TOwner LogInfo(string testMessage) {
            Log.Info(testMessage);
            return Owner;
        }
    }
}
