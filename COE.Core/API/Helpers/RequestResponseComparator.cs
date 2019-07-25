using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace COE.Core.API.Helpers
{
    public static class RequestResponseComparator
    {
        public static void IsOkResponseStatusCode(HttpResponseMessage response)
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        public static void Is500ResponseStatusCode(Func<Task<HttpResponseMessage>> act)
        {
            HttpRequestException ex = Assert.ThrowsAsync<HttpRequestException>(async ()
                => await act());
            Assert.That(ex.InnerException.Message.Contains("500"));
        }

        public static void Is401Response(Func<Task<HttpResponseMessage>> act)
        {
            HttpRequestException ex = Assert.ThrowsAsync<HttpRequestException>(async ()
                => await act());
            Assert.That(ex.InnerException.Message.Contains("401"));
            Assert.That(ex.InnerException.Message.Contains("Unauthorized"));
        }
    }
}
