using System;

namespace COE.Core.Logging
{
    /// <summary>
    /// Interface for logger class for HTTP requests
    /// </summary>
    public interface ICOEHttpClientLogger
    {
        /// <summary>
        /// Log http request starts executing.
        /// </summary>
        /// <param name="url">Url of API to call.</param>
        /// <param name="httpMethod">http method.</param>
        void LogRequestStarting(string url, string httpMethod);

        /// <summary>
        /// Log http request starts executing.
        /// </summary>
        /// <param name="url">Url of API was call.</param>
        /// <param name="httpMethod">http method.</param>
        /// <param name="requestBody">Request body (for POST/PUT requests)</param>
        void LogRequestStarting(string url, string httpMethod, string requestBody);

        /// <summary>
        /// Log HTTP request has finished executing.
        /// </summary>
        /// <param name="url">Url of API was call.</param>
        /// <param name="httpMethod">http method.</param>
        /// <param name="result">Result object.</param>
        /// <param name="responseLength">Length of Response content in bytes.</param>
        /// <param name="period">Time spent on request.</param>
        void LogRequestFinished(string url, string httpMethod, object result, int responseLength, TimeSpan period);

        /// <summary>
        /// Log HTTP request failed with exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        void LogHttpRequestFailed(Exception ex);
    }
}
