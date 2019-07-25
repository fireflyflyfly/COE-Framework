using System;

namespace COE.Core.Logging
{
    /// <summary>
    /// Logger class for DB requests
    /// </summary>
    public sealed class COEDbLogger : ICOEDbLogger
    {
        private readonly ICOEMainLogger _logger;

        /// <summary>
        /// Initializes new instance of <see cref="DbLogger"/> that writes its message into underlying <see cref="ICOEMainLogger"/>.
        /// </summary>
        /// <param name="logger">Instance of <see cref="IMainLogger"/> class.</param>
        public COEDbLogger(ICOEMainLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs DB connection has been created.
        /// </summary>
        public void LogDbRequestInitiating(string connectionString)
        {
            _logger.Log($"DB request initialized. Connection String: {connectionString}");
        }

        /// <summary>
        /// Logs DB request starts executing.
        /// </summary>
        public void LogDbRequestStarted(string request, string parameters)
        {
            _logger.Log($"DB request started. Query/SP: {request}, Parameters: {parameters}");
        }

        /// <summary>
        /// Logs DB request has finished executing.
        /// </summary>
        public void LogDbRequestFinished(string request, string parameters, object result)
        {
            _logger.Log($"DB request finished. Query/SP: {request}, Parameters: {parameters}\n Resuts: {result.ToString()}");
        }

        /// <summary>
        /// Logs DB request processing error of any type and reason.
        /// </summary>
        public void LogDbRequestError(Exception error)
        {
            _logger.Error($"DB request failed. Exception {error.ToString()}");
        }
    }
}
