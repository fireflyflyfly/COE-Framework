using COE.Core.Helpers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COE.Core.API.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent BuildFromStringContent(object value)
        {
            HttpContent results = new StringContent(JsonHelper.Serialize(value), Encoding.UTF8, "application/json");

            return results;
        } 

        public static async Task<T> GetObjectFromContent<T>(HttpContent content)
        {
            string respContent = await content.ReadAsStringAsync();
            T result = JsonHelper.DeserializeObject<T>(respContent);

            return result;
        }
    }
}
