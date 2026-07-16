namespace Sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public static class ObjectExtensions
    {
        private static readonly JsonSerializerOptions IndentedOptions = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public static void Dump(this object obj)
        {
            Console.WriteLine(JsonSerializer.Serialize(obj, IndentedOptions));
            if (obj is IEnumerable<object> collection)
            {
                Console.WriteLine("Total records: " + collection.Count());
            }
        }

        public static string ToJsonString(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
