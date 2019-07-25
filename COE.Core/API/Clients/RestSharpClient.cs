using RestSharp;
using RestSharp.Extensions;
using System.IO;
using System.Net;
using COE.Core.Helpers;
using COE.Core.Models;
using COE.Core.Exceptions;
using Atata.Configuration.Json;

namespace COE.Core.API.Clients
{
    public class RestSharpClient
    {
        private static RestClient Client;
        protected static AppData AppData;

        public static void SetUpRestClient()
        {
            AppData = new AppData();
            Client = new RestClient(JsonConfig.Current.BaseUrl);
            Client.Timeout = 90000;
            Client.ReadWriteTimeout = 60000;
            Client.CookieContainer = new CookieContainer();
            SetDefaultAuthenticationTokens();
        }

        public static void SetDefaultAuthenticationTokens()
        {
            SetUserAuthenticationTokens(AppData.RESTSharpUserName);
        }

        public static void SetUserAuthenticationTokens(string userName)
        {
            Client.DefaultParameters.Clear();
            var authenticationString = $"UserName={userName}&Password={AppData.RESTSharpPassword}&grant_type=password";
            var request = new RestRequest("token", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("text/plain", authenticationString, ParameterType.RequestBody);
            var response = Client.Execute(request);
            var responseJson = response.Content;

            if (response.StatusCode != HttpStatusCode.OK)
                throw response.ErrorException;

            Client.AddDefaultHeader("Authorization", "Bearer " + JsonHelper.GetJsonValueByKey(responseJson, "access_token"));
            Client.AddDefaultHeader("X-XSRF-Token", JsonHelper.GetJsonValueByKey(responseJson, "form_token"));
        }

        protected IRestResponse ExecuteWithoutMapping(RestRequest request)
        {
            return Client.Execute(request);
        }

        protected IRestResponse<COEResponseWrapper<T>> Execute<T>(RestRequest request)
        {
            var response = Client.Execute<COEResponseWrapper<T>>(request);
            if (response.Data.HasError || response.StatusCode != HttpStatusCode.OK || !response.Data.MessageType.Equals("success"))
                throw new COEResponseInvalidException(response.Content, request);
            return response;
        }

        protected IRestResponse<COEResponseWrapper<T>> ExecuteNoValidation<T>(RestRequest request) where T : new()
        {
            return Client.Execute<COEResponseWrapper<T>>(request);
        }

        protected void DownloadFile(RestRequest request, string downloadPath)
        {
            Client.DownloadData(request).SaveAs(downloadPath);
            if (!File.Exists(downloadPath)) throw new FileNotFoundException($"A file does not exist in a folder after downloading: {downloadPath}");
        }

        protected string UploadFile(RestRequest request, FileInfo file)
        {
            var response = ExecuteNoValidation<object>(request);

            var beginingOfFileName = Path.GetFileNameWithoutExtension(file.FullName);
            if (response.StatusCode != HttpStatusCode.OK || !response.Content.StartsWith($"\"{beginingOfFileName}"))
                throw new COEResponseInvalidException(response.Content, request);
            return response.Content.Replace("\"", "");
        }
    }
}