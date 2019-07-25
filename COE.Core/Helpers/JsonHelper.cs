using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;

namespace COE.Core.Helpers
{
    /// <summary>
    /// Class contains helper methods for JSON object serialization/deserialization.
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = BuildDefaultSettings();

        /// <summary>
        /// Gets new instance of default JSON serializer settings for reuse.
        /// </summary>
        public static JsonSerializerSettings DefaultSettings
        {
            get { return BuildDefaultSettings(); }
        }

        /// <summary>
        ///  Serializes the specified object to a JSON string using defined JsonSerializerSettings
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, JsonSerializerSettings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, JsonSerializerSettings);
        }

        /// <summary>
        /// Deserializes data from Stream to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="stream">The Stream to deserialize</param>
        /// <returns>The deserialized object from the Stream.</returns>
        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream, Encoding.UTF8))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        public static bool IsNullOrEmpty(string text)
        {
            JToken obj = JToken.Parse(text);

            return (obj == null) ||
               (obj.Type == JTokenType.Array && !obj.HasValues) ||
               (obj.Type == JTokenType.Object && !obj.HasValues) ||
               (obj.Type == JTokenType.String && obj.ToString() == string.Empty) ||
               (obj.Type == JTokenType.Null);
        }

        private static JsonSerializerSettings BuildDefaultSettings()
        {
            var result = new JsonSerializerSettings();

            result.Converters.Add(new IsoDateTimeConverter());
            result.Converters.Add(new StringEnumConverter());
            result.TypeNameHandling = TypeNameHandling.None;

            return result;
        }
        //Get value of a key from JSON string
        public static string GetJsonValueByKey(string jsonContent, string jsonKey)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent, settings)[jsonKey].ToString();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
        //Get value of a key from JSON string when JSON contains some sub-sections
        public static string GetJsonValueFromSectionByKey(string jsonContent, string section, string jsonKey)
        {
            var sectionJson = GetJsonValueByKey(jsonContent, section);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(sectionJson)[jsonKey].ToString();
        }

        public static JObject GetJobjectFromJArray(JArray jarray, string property, string value)
        {
            return jarray.Children<JObject>().FirstOrDefault(o => o[property].ToString() == value);
        }
    }
}
