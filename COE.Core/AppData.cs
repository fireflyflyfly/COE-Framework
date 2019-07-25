using Atata.Configuration.Json;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace COE.Core
{
    public class AppData : AppData<AppData> {

    }

    public abstract class AppData<TConfig> : JsonConfig<TConfig> where TConfig : JsonConfig<TConfig>
    {
        public string PathToOutputDir { get; set; }
        public CultureInfo Cultureinfo { get; set; }
        public string AuthenticationString { get; set; }
        public string RESTSharpUserName { get; set; }
        public string RESTSharpPassword { get; set; }
        public string AuthorizationToken { get; set; }
        public bool OneTimeDriverSetup { get; set; }
        public bool CustomBuildDriver { get; set; }
        public bool ZephyrEnabled { get; set; }
        public string ZephyrProjectID { get; set; }
        public string ZephyrVersionID { get; set; }
        public string ZephyrCycleID { get; set; }
        public string ZephyrUserID { get; set; }
        public string ZephyrBaseUrl { get; set; }
        public string ZephyrSecretKey { get; set; }
        public string ZephyrZAPIAccessKey { get; set; }
        public int ZephyrTimeout { get; set; }
        public int ZephyrReadWriteTimeout { get; set; }

        public string DateTimeNowId => DateTime.Now.ToString("MMddHHmmFF").PadRight(10, '0');

        public AppData()
        {
            PathToOutputDir = new FileInfo(GetType().Assembly.Location).DirectoryName;
            Cultureinfo = new CultureInfo("en");
            RESTSharpUserName = ConfigurationManager.AppSettings["RESTSharpUserName"];
            RESTSharpPassword = ConfigurationManager.AppSettings["RESTSharpPassword"];
            AuthenticationString = $"UserName={RESTSharpUserName}&Password={RESTSharpPassword}&grant_type=password";
            AuthorizationToken = ConfigurationManager.AppSettings["AuthorizationToken"];
            OneTimeDriverSetup = (ConfigurationManager.AppSettings["OneTimeDriverSetup"] =="true");
            CustomBuildDriver = (ConfigurationManager.AppSettings["CustomBuildDriver"] =="true");
            ZephyrEnabled = (ConfigurationManager.AppSettings["Zephyr.Enabled"]=="true");
            ZephyrProjectID = ConfigurationManager.AppSettings["Zephyr.projectID"];
            ZephyrVersionID = ConfigurationManager.AppSettings["Zephyr.versionID"];
            ZephyrCycleID = ConfigurationManager.AppSettings["Zephyr.cycleID"];
            ZephyrUserID = ConfigurationManager.AppSettings["Zephyr.userID"];
            ZephyrBaseUrl = ConfigurationManager.AppSettings["Zephyr.BaseUrl"];
            ZephyrSecretKey = ConfigurationManager.AppSettings["Zephyr.secretKey"];
            ZephyrZAPIAccessKey = ConfigurationManager.AppSettings["Zephyr.zapiAccessKey"];
            ZephyrTimeout = int.Parse(ConfigurationManager.AppSettings["Zephyr.Timeout"]);
            ZephyrReadWriteTimeout = int.Parse(ConfigurationManager.AppSettings["Zephyr.ReadWriteTimeout"]);
        }
    }
}