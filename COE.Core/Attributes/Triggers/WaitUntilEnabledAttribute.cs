namespace Atata
{
    public class WaitUntilEnabledAttribute : TriggerAttribute
    {
        private double? timeout;

        private double? retryInterval;

        public WaitUntilEnabledAttribute(TriggerEvents on = TriggerEvents.BeforeSet | TriggerEvents.BeforeClick,
            TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the waiting timeout in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingTimeout.TotalSeconds</c>.
        /// </summary>
        public double Timeout
        {
            get { return timeout ?? (AtataContext.Current?.WaitingTimeout ?? RetrySettings.Timeout).TotalSeconds; }
            set { timeout = value; }
        }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// The default value is taken from <c>AtataContext.Current.WaitingRetryInterval.TotalSeconds</c>.
        /// </summary>
        public double RetryInterval
        {
            get { return retryInterval ?? (AtataContext.Current?.WaitingRetryInterval ?? RetrySettings.Interval).TotalSeconds; }
            set { retryInterval = value; }
        }

        protected override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ((Control<TOwner>)context.Component).Should.Within(Timeout, RetryInterval).BeEnabled();
        }
    }
}