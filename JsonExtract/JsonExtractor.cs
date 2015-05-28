using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace JsonExtract
{
    static class JsonExtractor
    {
        /// <summary>
        /// Extracts the objects.
        /// </summary>
        /// <param name="jsonArrayPath">The json array path.</param>
        /// <param name="propertyPaths">The property paths.</param>
        /// <param name="inputPath">The input path.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">Provided array path does not contain Json array.</exception>
        public static List<List<string>> ExtractObjects(string jsonArrayPath, string[] propertyPaths, string inputPath)
        {
            JObject json = JObject.Parse(File.ReadAllText(inputPath));

            JArray array = json.SelectToken(jsonArrayPath) as JArray;
            if (array == null)
            {
                throw new InvalidCastException("Provided array path does not contain Json array.");
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
                        JArray arrayProperty = (JArray) property;

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

                    // Value types can be added as plain strings
                    properties.Add(property.Value<string>());
                }

                objects.Add(properties);
            }

            return objects;
        } 
    }
}
