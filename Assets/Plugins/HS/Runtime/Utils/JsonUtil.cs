using Newtonsoft.Json;

namespace HS
{
    /// <summary>
    /// Json工具类，必须使用PackageManager中的NewtonsoftJson，与js的JSON类似，安全无痛
    /// </summary>
    public class JsonUtil
    {
        public static string ToJson<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return json;
        }

        public static T FromJson<T>(string str)
        {
            var data = JsonConvert.DeserializeObject<T>(str);
            return data;
        }
    }
}
