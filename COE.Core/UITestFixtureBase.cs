using COE.Core.Helpers;
using NUnit.Framework;
using NUnit.Allure.Core;
using System;
using System.Reflection;
using NUnit.Framework.Interfaces;
using COE.Core.Attributes;
using Allure.Commons;
using Atata;

/// <summary>
/// Core of Testing automation package 
/// </summary>
namespace COE.Core
{
    /// <summary>
    /// NUnit Base Test Fixture Class.
    /// Have also Allure attribute.
    /// </summary>
    [TestFixture]
    [AllureNUnit]
    public class UITestFixtureBase
    {
        public CommonHelper _commonHelper = new CommonHelper();
        public bool SkipBaseBuild = false;

        /// <summary>
        ///   Setup method for Test Fixture. Creates Atata Context builder, configure it and build
        /// </summary>
        [SetUp]
        public void SetUpEachTest()
        {
            if (!General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver && !SkipBaseBuild)
            {
                General.BuildContext().Build();
            }
        }

        /// <summary>
        ///   Tear down method for Test Fixture. Implements Zephyr integration if enabled.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (General.AppData.ZephyrEnabled) {
                string className = TestContext.CurrentContext.Test.ClassName;
                string[] parts = className.Split('.');
                Type clsType = Assembly.Load(parts[0]).GetType(className, true);
                MethodInfo mInfo = clsType.GetMethod(TestContext.CurrentContext.Test.MethodName);
                bool isDef = Attribute.IsDefined(mInfo, typeof(ZephyrTestAttribute));
                if (isDef)
                {
                    ZephyrTestAttribute ztAttr = (ZephyrTestAttribute)Attribute.GetCustomAttribute(mInfo, typeof(ZephyrTestAttribute));
                    if (ztAttr != null)
                    {

                        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                        {
                            General.ZAPIManager.CreateExecution(ztAttr.Value, 2);//2 - FAIL
                        }
                        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                        {
                            General.ZAPIManager.CreateExecution(ztAttr.Value, 1);//1 - PASS
                        }
                        //3 - WORK IN PROGRESS, 4 - BLOCKED
                    }
                }
            }
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                //Add screenshot to Allure on Failure
                AllureLifecycle.Instance.AddAttachment($"Screenshot [{DateTime.Now:HH:mm:ss}]",
                "image/png",
                AtataContext.Current.Driver.GetScreenshot().AsByteArray);
            }
            if (!General.AppData.OneTimeDriverSetup && !General.AppData.CustomBuildDriver && !SkipBaseBuild)
            {
                AtataContext.Current?.CleanUp();
            }
        }
    }
}
