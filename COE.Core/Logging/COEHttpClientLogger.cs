using System;
using System.Text;

namespace COE.Core.Logging
{
    /// <summary>
    /// Logger class for HTTP requests
    /// </summary>
    public class COEHttpClientLogger : ICOEHttpClientLogger
    {
        private readonly ICOEMainLogger _logger;

        /// <summary>
        /// Initializes new instance of <see cref="DbLogger"/> that writes its message into underlying <see cref="ICOEMainLogger"/>.
        /// </summary>
        /// <param name="logger">Instance of <see cref="IMainLogger"/> class.</param>
        public COEHttpClientLogger(ICOEMainLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Log http request starts executing.
        /// </summary>
        /// <param name="url">Url of API to call.</param>
        /// <param name="httpMethod">http method.</param>
        public void LogRequestStarting(string url, string httpMethod)
        {
            _logger.Log($"API {httpMethod} request starting. Url: {url}");
        }

        /// <summary>
        /// Log http request starts executing.
        /// </summary>
        /// <param name="url">Url of API was call.</param>
        /// <param name="httpMethod">http method.</param>
        /// <param name="requestBody">Request body (for POST/PUT requests)</param>
        public void LogRequestStarting(string url, string httpMethod, string requestBody)
        {
            _logger.Log($"API {httpMethod} request starting. Url: {url}. Request body: {requestBody}");
        }

        /// <summary>
        /// Log HTTP request has finished executing.
        /// </summary>
        /// <param name="url">Url of API was call.</param>
        /// <param name="httpMethod">http method.</param>
        /// <param name="result">Result object.</param>
        /// <param name="responseLength">Length of Response content in bytes.</param>
        /// <param name="period">Time spent on request.</param>
        public void LogRequestFinished(string url, string httpMethod, object result, int responseLength, TimeSpan period)
        {
            _logger.Log($"API {httpMethod} request finished. Url: {url}, Response length: {responseLength}, Time spent: {period.ToString("c")}");
        }

        /// <summary>
        /// Log HTTP request failed with exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public void LogHttpRequestFailed(Exception ex)
        {
            _logger.Error($"Http call failed. Exception: {FormatExceptionMessage(ex)}");
        }

        private string FormatExceptionMessage(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.AppendFormat("\nSource: {0} \nMessage: {1} \nStackTrace: \n{2}",
                ex.Source,
                ex.Message,
                ex.StackTrace);

            return errorMessage.ToString();
        }
    }
}
