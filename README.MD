# JsonExtract

Have you ever found yourself from situation where you have one or many json files and you want
to parse specific array of data (etc. comments, movie titles, song names)? I have found myself from there multiple times
and finally decided to make permantent solution instead of writing custom parser everytime this functionality is needed.

JsonExtract simply takes your file(s) as command line arguments and lets you specify array containing wanted 
objects and properties of the arrays. Properties are conveniently saved to csv file after extraction is complete.

## Usage
---------

Using JsonExtract is simple. To get available commands, use:

	JsonExtract

To extract properties from single file, use:

	JsonExtract file.json --array=$.posts --properties=$.title,$.date,$.comment --output=out.csv

To extract properties from multiple files with same structure, use:

	JsonExtract C:\comments --array=$.posts --properties=$.title,$.date,$.comment --output=out.csv

Notice that properties JSONPaths are relative to root of of the object inside specified array.

## Requirements

* .NET Framework 4.5

## Binaries

Latest builds can be found from [here](http://uuksu.kapsi.fi/other/JsonExtract/).

