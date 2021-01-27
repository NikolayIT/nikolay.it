namespace Sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public static class ObjectExtensions
    {
        public static void Dump(this object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented, new StringEnumConverter()));
            if (obj is IEnumerable<object> collection)
            {
                Console.WriteLine("Total records: " + collection.Count());
            }
        }

        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }
    }
}
