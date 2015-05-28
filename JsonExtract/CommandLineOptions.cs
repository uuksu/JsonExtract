using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace JsonExtract
{
    class CommandLineOptions
    {
        [ValueList(typeof(List<string>), MaximumElements = 1)]
        public IList<string> InputPaths { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output CSV file path.")]
        public string OutputPath { get; set; }

        [Option('r', "array", Required = true, HelpText = "Name of the array containing objects wanted to parse.")]
        public string JsonObjectArrayPath { get; set; }

        [Option('p', "properties", Required = true, HelpText = "PropertyPaths wanted to extract. JSONPath strings divided by comma.")]
        public string PropertyPaths { get; set; }

        [Option('a', "append", Required = false, HelpText = "If set true values are appended to output file.")]
        public bool Append { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(entryAssembly.Location);

            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("JsonExtract", fvi.FileVersion),
                Copyright = new CopyrightInfo("Mikko Uuksulainen", 2015),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine(Environment.NewLine);
            help.AddPreOptionsLine("Usage: JsonExtract <input json file/directory containing json files> [options]");
            help.AddOptions(this);

            return help;
        }
    }
}
