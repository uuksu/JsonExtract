using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace JsonExtract
{
    class CommandLineOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input file that is processed.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path.")]
        public string OutputPath { get; set; }

        [Option('a', "array", Required = true, HelpText = "Name of the array containing objects wanted to parse.")]
        public string JsonObjectArrayPath { get; set; }

        [Option('p', "properties", Required = true, HelpText = "PropertyPaths wanted to extract.")]
        public string PropertyPaths { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("JsonExtract");
            return usage.ToString();
        }
    }
}
