using System;

namespace COE.Core.Logging
{
    /// <summary>
    /// Interface for general purpose logger.
    /// </summary>
    public interface ICOEMainLogger
    {
        /// <summary>
        /// Log Exception in testing framework.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        void Exception(Exception ex, object attachment);

        /// <summary>
        /// Log Error message.
        /// </summary>
        /// <param name="message">Error message to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        void Error(string message, object attachment = null);

        /// <summary>
        /// Log general informational message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        void Log(string message, object attachment = null);
    }
}
