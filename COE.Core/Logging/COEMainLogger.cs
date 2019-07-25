using System;
using System.Reflection;

namespace COE.Core.Logging
{
    /// <summary>
    /// General purposes logger.
    /// </summary>
    public sealed class COEMainLogger : ICOEMainLogger
    {
        private readonly MethodInfo writeMethod;
        private static string Dt => $"{DateTime.Now:hh:mm:ss dd.MM.yy}";

        /// <summary>
        /// Creates new instance of the <see cref="COEMainLogger"/>.
        /// </summary>
        public COEMainLogger()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);
            writeMethod = testContextType.GetMethod("WriteLine", new[] { typeof(string)});
        }

        /// <summary>
        /// Log Exception in testing framework.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        public void Exception(Exception ex, object attachment = null)
        {
            //TestContext.Out.WriteLine($"Exception");
            writeMethod.InvokeStatic($"[Exception: {Dt}] {ex.ToString()}");
        }

        /// <summary>
        /// Log Error message.
        /// </summary>
        /// <param name="message">Error message to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        public void Error(string message, object attachment = null)
        {
            //TestContext.WriteLine($"[Error: {Dt}] {message}");
            writeMethod.InvokeStatic($"[Error: {Dt}] {message}");
        }

        /// <summary>
        /// Log general informational message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="attachment">Data object to attach to the message.</param>
        public void Log(string message, object attachment = null)
        {
            //TestContext.Progress.WriteLine($"[Debug: {Dt}] {message}");
            writeMethod.InvokeStatic($"[Log: {Dt}] {message}");
        }
    }
}
