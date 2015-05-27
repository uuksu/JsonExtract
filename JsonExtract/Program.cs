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
        private static CommandLineOptions commandLineOptions;

        /// <summary>
        /// Writes the extracted json objects to csv file.
        /// </summary>
        /// <param name="extractedObjects">The extracted objects.</param>
        /// <param name="outputPath">The output path.</param>
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

        /// <summary>
        /// Extracts specified input file or all json files from specified input directory.
        /// </summary>
        private static List<List<string>> ExtractFiles()
        {
            List<string> paths = new List<string>();

            if (String.IsNullOrEmpty(commandLineOptions.InputPath))
            {
                if (String.IsNullOrEmpty(commandLineOptions.InputDirectoryPath))
                {
                    throw new Exception("No input path specified.");
                }

                DirectoryInfo di = new DirectoryInfo(commandLineOptions.InputDirectoryPath);

                FileInfo[] fileInfos = di.GetFiles("*.json");

                paths.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));
            }
            else
            {
                paths.Add(commandLineOptions.InputPath);
            }

            List<List<string>> extractedObjects = new List<List<string>>();

            foreach (string path in paths)
            {
                string[] propertyPaths = commandLineOptions.PropertyPaths.Split(',');
                extractedObjects.AddRange(JsonExtractor.ExtractObjects(commandLineOptions.JsonObjectArrayPath,
                    propertyPaths, path));
            }

            return extractedObjects;
        }

        static void Main(string[] args)
        {
            commandLineOptions = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, commandLineOptions) == false)
            {
                Environment.Exit(1);
            }

            csvConfiguration = new CsvConfiguration
            {
                Delimiter = ","
            };

            List<List<string>> extractedObjects = ExtractFiles();

            WriteOutput(extractedObjects, commandLineOptions.OutputPath);
        }
    }
}
