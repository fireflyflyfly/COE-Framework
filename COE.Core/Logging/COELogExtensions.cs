using System.Reflection;

namespace COE.Core.Logging
{
    /// <summary>
    /// Utility class for logging
    /// </summary>
    public static class COELogExtensions
    {
        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="method">Method.</param>
        /// <param name="args">Parameters.</param>
        /// <returns></returns>
        public static object InvokeStatic(this MethodInfo method, params object[] args)
        {
            return method.Invoke(null, args);
        }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <typeparam name="T">Type of returning value.</typeparam>
        /// <param name="method">Method.</param>
        /// <param name="args">Parameters.</param>
        /// <returns></returns>
        public static T InvokeStatic<T>(this MethodInfo method, params object[] args)
        {
            return (T)method.Invoke(null, args);
        }
    }
}
