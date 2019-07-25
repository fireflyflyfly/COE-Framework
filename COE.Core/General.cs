using Atata;
using COE.Core.Report.ZephyrAPI;
using System;
using Atata.Configuration.Json;

namespace COE.Core
{
    public static class General
    {
        public static AppData AppData;
        public static ZAPIManager ZAPIManager;
        public static bool ConfigApplied = false;

        static General()
        {
            AppData = new AppData();
            if (AppData.ZephyrEnabled)
            {
                ZAPIManager = new ZAPIManager();
            }
        }

        /// <summary>
        ///   Build Atata Context method
        /// </summary>
        public static AtataContextBuilder BuildContext()
        {
            return AtataContext.Configure()
                .UseNUnitTestName()
                .AddScreenshotFileSaving()
                .WithFolderPath(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\{build-start}\{test-name}");
        }

    }
}