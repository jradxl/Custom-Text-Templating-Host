﻿///
/// From http://msdn.microsoft.com/en-us/library/bb126579.aspx
/// But example is inaccurate.
/// This works for VS2012
///
namespace CustomTextTemplatingHostDemo
{
    using CustomTextTemplatingHost;
    using Microsoft.VisualStudio.TextTemplating;
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    //This will accept the path of a text template as an argument.
    //It will create an instance of the custom host and an instance of the
    //text templating transformation engine, and will transform the
    //template to create the generated text output file.
    //-------------------------------------------------------------------------
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessTemplate(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("If the Transform runs correctly,\nyou will see your output file in same folder as your source.");
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        static void ProcessTemplate(string[] args)
        {
            string templateFileName = null;
            if (args.Length == 0)
            {
                throw new System.Exception("you must provide a text template file path");
            }
            templateFileName = args[0];
            if (templateFileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }
            if (!File.Exists(templateFileName))
            {
                throw new FileNotFoundException("the file cannot be found");
            }

            CustomCmdLineHost host = new CustomCmdLineHost();
            Engine engine = new Engine();

            host.TemplateFile = templateFileName;

            //Read the text template.
            string input = File.ReadAllText(templateFileName);
            //Transform the text template.
            string output = engine.ProcessTemplate(input, host);
            string outputFileName = Path.GetFileNameWithoutExtension(templateFileName);
            outputFileName = Path.Combine(Path.GetDirectoryName(templateFileName), outputFileName);
            outputFileName = outputFileName + "1" + host.FileExtension;
            File.WriteAllText(outputFileName, output, host.FileEncoding);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ToString());
            }
        }
    }
}
