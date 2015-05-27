using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace JsonExtract
{
    class Program
    {
        private static CsvConfiguration csvConfiguration;

        private static void WriteOutput(List<List<string>> extractedObjects, string outputPath)
        {
            using (TextWriter textWriter = new StreamWriter(outputPath))
            {
                using (var csv = new CsvWriter(textWriter, csvConfiguration))
                {
                    foreach (List<string> extractedObjectProperties in extractedObjects)
                    {
                        foreach (string property in extractedObjectProperties)
                        {
                            csv.WriteField(property);
                        }
                        csv.NextRecord();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options) == false)
            {
                Environment.Exit(1);
            }

            csvConfiguration = new CsvConfiguration
            {
                Delimiter = ","
            };

            string[] propertyPaths = options.PropertyPaths.Split(',');

            List<List<string>> objects = JsonExtractor.ExtractObjects(options.JsonObjectArrayPath, propertyPaths, options.InputPath);

            WriteOutput(objects, options.OutputPath);
        }
    }
}
