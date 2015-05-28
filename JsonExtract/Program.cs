using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CsvHelper;
using CsvHelper.Configuration;

namespace JsonExtract
{
    class Program
    {
        private static CsvConfiguration _csvConfiguration;
        private static CommandLineOptions _commandLineOptions;

        /// <summary>
        /// Writes the extracted json objects to csv file.
        /// </summary>
        /// <param name="extractedObjects">The extracted objects.</param>
        /// <param name="outputPath">The output path.</param>
        private static void WriteOutput(List<List<string>> extractedObjects, string outputPath)
        {
            using (TextWriter textWriter = new StreamWriter(outputPath, _commandLineOptions.Append))
            {
                using (var csv = new CsvWriter(textWriter, _csvConfiguration))
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

            if (Directory.Exists(_commandLineOptions.InputPaths.First()))
            {
                DirectoryInfo di = new DirectoryInfo(_commandLineOptions.InputPaths.First());

                FileInfo[] fileInfos = di.GetFiles("*.json");

                paths.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));
            }
            else
            {
                if (!File.Exists(_commandLineOptions.InputPaths.First()))
                {
                    throw new FileNotFoundException("Input path does not exists");
                }

                paths.Add(_commandLineOptions.InputPaths.First());
            }

            List<List<string>> extractedObjects = new List<List<string>>();

            foreach (string path in paths)
            {
                string[] propertyPaths = _commandLineOptions.PropertyPaths.Split(',');
                extractedObjects.AddRange(JsonExtractor.ExtractObjects(_commandLineOptions.JsonObjectArrayPath,
                    propertyPaths, path));
            }

            return extractedObjects;
        }

        static void Main(string[] args)
        {
            _commandLineOptions = new CommandLineOptions();

            if (Parser.Default.ParseArguments(args, _commandLineOptions) == false)
            {
                Environment.Exit(1);
            }

            if (_commandLineOptions.InputPaths.Count == 0)
            {
                Console.WriteLine("Please provide input path.");
                Environment.Exit(1);
            }

            _csvConfiguration = new CsvConfiguration
            {
                Delimiter = ","
            };

            List<List<string>> extractedObjects = null;

            try
            {
                extractedObjects = ExtractFiles();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Specified input path does not exists.");
                Environment.Exit(1);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
            WriteOutput(extractedObjects, _commandLineOptions.OutputPath);
        }
    }
}
