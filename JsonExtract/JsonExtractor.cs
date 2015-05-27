using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonExtract
{
    static class JsonExtractor
    {
        /// <summary>
        /// Extracts the objects and and specified properties from specified json array in input file.
        /// </summary>
        /// <param name="jsonArrayPath">The json array path.</param>
        /// <param name="propertyPaths">The property string.</param>
        /// <param name="inputPath">The input path.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Not array.</exception>
        public static List<List<string>> ExtractObjects(string jsonArrayPath, string[] propertyPaths, string inputPath)
        {
            JObject json = JObject.Parse(File.ReadAllText(inputPath));

            JArray array = json.SelectToken(jsonArrayPath) as JArray;
            if (array == null)
            {
                throw new Exception("Not array.");
            }

            List<List<string>> objects = new List<List<string>>();

            foreach (JToken jObject in array)
            {
                List<string> properties = new List<string>();

                foreach (string propertyPath in propertyPaths)
                {
                    JToken property = jObject.SelectToken(propertyPath);

                    // Arrays must parsed differently so that they can be extracted as list format
                    if (property.Type == JTokenType.Array)
                    {
                        StringBuilder arrayStringBuilder = new StringBuilder();
                        JArray arrayProperty = property as JArray;

                        for (int i = 0; i < arrayProperty.Count; i++)
                        {
                            JToken arrayPropertyItem = arrayProperty[i];
                            arrayStringBuilder.Append(arrayPropertyItem.Value<string>());

                            if (i != arrayProperty.Count - 1)
                            {
                                arrayStringBuilder.Append(';');
                            }
                        }

                        properties.Add(arrayStringBuilder.ToString());

                        continue;
                    }

                    // Value types can be added as strings
                    properties.Add(property.Value<string>());
                }

                objects.Add(properties);
            }

            return objects;
        } 
    }
}
