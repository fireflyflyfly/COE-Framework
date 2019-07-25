using System.Net.NetworkInformation;

namespace COE.Core.Helpers
{
    /// <summary>
    /// Class contains helper methods for network interaction.
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Get results of PINGing specified host
        /// </summary>
        /// <param name="host">Host name</param>
        /// <returns><c>true</c> - if ping was successful, <c>false</c> - if was not.</returns>
        public static bool PingHost(string host)
        {
            bool pingable = false;

            using (Ping pingSender = new Ping())
            {
                try
                {
                    PingReply reply = pingSender.Send(host, 200);
                    pingable = reply.Status == IPStatus.Success;
                }
                catch (PingException)
                {
                    // Discard PingExceptions and return false;
                }
            }

            return pingable;
        }
    }
}
