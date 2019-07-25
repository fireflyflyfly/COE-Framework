using System;

namespace COE.Core.Logging
{
    /// <summary>
    /// Interface for Logger that logs database requests.
    /// </summary>
    public interface ICOEDbLogger
    {
        /// <summary>
        /// Logs DB connection has been created.
        /// </summary>
        void LogDbRequestInitiating(string connectionString);

        /// <summary>
        /// Logs DB request starts executing.
        /// </summary>
        void LogDbRequestStarted(string request, string parameters);

        /// <summary>
        /// Logs DB request has finished executing.
        /// </summary>
        void LogDbRequestFinished(string request, string parameters, object result);

        /// <summary>
        /// Logs DB request processing error of any type and reason.
        /// </summary>
        void LogDbRequestError(Exception error);
    }
}
