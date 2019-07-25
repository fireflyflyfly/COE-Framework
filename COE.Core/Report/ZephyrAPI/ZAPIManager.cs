using Atata;
using COE.Core.Helpers;
using JWT.Algorithms;
using JWT.Builder;
using RestSharp;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace COE.Core.Report.ZephyrAPI
{
    /// <summary>
    /// Class for managing API calls to Zephyr of JIRA Cloud
    /// Have wrapper for Zephyr methods for creating execution, update execution, generating GWT for authorization in Zephyr
    /// prepare JSON body for API call methods, get server information.
    /// 
    /// @author Ivan.Yankovyi @teaminternational.com
    /// @version 1.0 25/05/19
    /// </summary>
    public class ZAPIManager
    {
        /// <summary>
        /// Project ID in JIRA for ZAPI. Can be found in JIRA Dashboard in the project management "Project Settings" / "Details" in URL parameter pid. Should look like "10000" or "10001", etc.  See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        public string projectID;
        /// <summary>
        /// Version ID in JIRA for ZAPI. Can be found in JIRA Dashboard in the project management "Project Settings" / "Versions" / click on the created version. Version ID will be in URL after "/versions/". Should look like "10000" or "10001", etc. See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        private string versionID;

        /// <summary>
        /// Version ID in JIRA for ZAPI. Can be found in JIRA Dashboard in the project management "Project Settings" / "Versions" / click on the created version. Version ID will be in URL after "/versions/". Should look like "10000" or "10001", etc. See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        private string cycleID;

        /// <summary>
        /// User ID for authorisation in ZAPI.  Can be found in URL inJIRA by visiting profile of user.  See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        private string userid;
        /// <summary>
        /// Access key for authorisation in ZAPI. Can be found in JIRA Dashboard in JIRA Settings / Apps / ZAPI.  See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        private string zapiAccessKey;
        /// <summary>
        /// Secret key for authorisation in ZAPI. Can be found in JIRA Dashboard in JIRA Settings / Apps / ZAPI.  See screenshots in <a href="/">"Overview"</a>.
        /// </summary>
        private string secretKey;
        /// <summary>
        /// Base URL for ZAPI calls. It can be found in ZAPI documentation. At the moment it's https://prod-api.zephyr4jiracloud.com/connect
        /// </summary>
        private string zephyrBaseUrl;

        /// <summary>The RestSharp client. Used for API calls.</summary>
        private RestClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZAPIManager"/> class.
        /// Loads config data nd initialize RestSharp client default varables and headers
        /// </summary>
        public ZAPIManager() {
            projectID = General.AppData.ZephyrProjectID;
            versionID = General.AppData.ZephyrVersionID;
            cycleID = General.AppData.ZephyrCycleID;
            userid = General.AppData.ZephyrUserID;
            zephyrBaseUrl = General.AppData.ZephyrBaseUrl;
            secretKey = General.AppData.ZephyrSecretKey;
            zapiAccessKey = General.AppData.ZephyrZAPIAccessKey;
            _client = new RestClient(zephyrBaseUrl);
            _client.Timeout = General.AppData.ZephyrTimeout;
            _client.ReadWriteTimeout = General.AppData.ZephyrReadWriteTimeout;
            _client.CookieContainer = new CookieContainer();
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("Content-Type", "application/json");
            _client.AddDefaultHeader("zapiAccessKey", zapiAccessKey);
            _client.AddDefaultHeader("User-Agent", "ZAPI");
        }

        /// <summary>
        /// Get Query String Hash
        /// </summary>
        /// <param name="qstring">Query string</param>
        /// <returns>Hash code for query string</returns>
        static string GetQSH(string qstring)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(qstring));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Get JSON Web Token for defined requestType and URL for 360 seconds
        /// </summary>
        /// <param name="requestType">Type of reuest</param>
        /// <param name="url">URL</param>
        /// <param name="queryString">Query string</param>
        /// <returns>generated JWT</returns>
        private string GetJWTToken(string requestType, string url, string queryString)
        {
            //define Expire Time
            var EXPIRE_TIME = 3600;
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var issueTime = DateTime.Now;
            var iat = (long)issueTime.Subtract(utc0).TotalMilliseconds;
            var exp = (long)issueTime.AddMilliseconds(EXPIRE_TIME).Subtract(utc0).TotalMilliseconds;
            string canonical_path = requestType + "&" + url + "&" + queryString;
            string token = new JwtBuilder()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret(secretKey)
              .AddClaim("sub", userid)
              .AddClaim("qsh", GetQSH(canonical_path))
              .AddClaim("iss", zapiAccessKey)
              .AddClaim("iat", iat)
              .AddClaim("exp", exp)
              .Build();
            return token;
        }

        /// <summary>
        /// Get Server (Zephyr for JIRA Cloud) Information.
        /// </summary>
        /// <returns>Information about Zephyr server in JSON format</returns>
        public string GetServerInfo()
        {
            string path = "/public/rest/api/1.0/serverinfo";
            string fullPath = "/connect" + path;
            string gwtToken = GetJWTToken("GET", path, "");
            string responseJson = "";
            _client.AddDefaultHeader("Authorization", "JWT " + gwtToken);
            try
            {
                var request = new RestRequest(fullPath, Method.GET) { RequestFormat = DataFormat.Json };
                responseJson = _client.Execute(request).Content;
            }
            catch (Exception e)
            {
                AtataContext.Current.Log.Error(e.Message);
            }
            return responseJson;
        }


        /// <summary>Prepares the json for creating new execution call.</summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="status">The status of execution.</param>
        /// <returns>Serialized JSON</returns>
        public string prepareJsonForExecution(string issueId, int status)
        {
            object jsonObj = new
            {
                issueId,
                cycleId = cycleID,
                status = new {
                    id = status
                },
                versionId = versionID,
                projectId = projectID,
                assigneeType = "currentUser"
            };
            return JsonHelper.Serialize(jsonObj);
        }

        /// <summary>Prepares the json for updaeting execution information (status and commentary).</summary>
        /// <param name="executionID">The execution identifier.</param>
        /// <param name="comment">The comment text.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="status">The status of test execution.</param>
        /// <returns>Serialized JSON</returns>
        public string prepareJsonForExecutionUpdate(string executionID, string comment, string issueId, int status) {
            object jsonObj = new
            {
                id = executionID,
                issueId = issueId,
                cycleId = cycleID,
                comment = comment,
                status = new
                {
                    id = status
                },
                versionId = versionID,
                projectId = projectID,
            };
            return JsonHelper.Serialize(jsonObj);
        }

        /// <summary>Makes API call to Zephyr to creates new execution for specified test id and write test execution status.</summary>
        /// <param name="testId">The test identifier.</param>
        /// <param name="status">The status of test exexution.</param>
        public void CreateExecution(string testId, int status)
        {
            string path = "/public/rest/api/1.0/execution";
            string fullPath = "/connect" + path;
            string gwtToken = GetJWTToken("POST", path, "");
            string responseJson = "";
            try
            {
                string requestJson = prepareJsonForExecution(testId, status);
                var request = new RestRequest(fullPath, Method.POST) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", "JWT " + gwtToken);
                request.AddParameter("application/json", requestJson, ParameterType.RequestBody);
                responseJson = _client.Execute(request).Content;
                string executionId = JsonHelper.GetJsonValueFromSectionByKey(responseJson, "execution", "id");
                gwtToken = GetJWTToken("PUT", path + "/" + executionId, "");
                _client.AddDefaultHeader("Authorization", "JWT " + gwtToken);
                requestJson = prepareJsonForExecutionUpdate(executionId, (status==1?"Test passed":"Test failed"), testId, status);
                request = new RestRequest(fullPath+"/"+ executionId, Method.PUT) { RequestFormat = DataFormat.Json };
                request.AddHeader("Authorization", "JWT " + gwtToken);
                request.AddParameter("application/json", requestJson, ParameterType.RequestBody);
                responseJson = _client.Execute(request).Content;
            }
            catch (Exception e)
            {
                AtataContext.Current.Log.Error(e.Message);
            }
        }
    }
}
