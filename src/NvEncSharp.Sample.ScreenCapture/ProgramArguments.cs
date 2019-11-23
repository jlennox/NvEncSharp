using System;

namespace Lennox.NvEncSharp.Sample.ScreenCapture
{
    internal class ProgramArguments
    {
        public string DisplayName { get; set; }
        public string OutputPath { get; set; }

        public ProgramArguments(string[] args)
        {
            for (var i = 0; i < args.Length; ++i)
            {
                string GetNextArgument()
                {
                    if (i + 1 > args.Length - 1)
                    {
                        throw new ArgumentNullException(
                            args[i], "Arguming required.");
                    }

                    return args[++i];
                }

                switch (args[i])
                {
                    case "--display":
                    case "-d":
                        DisplayName = GetNextArgument();
                        break;
                    case "--output":
                    case "-o":
                        OutputPath = GetNextArgument();
                        break;
                }
            }

            if (OutputPath == null)
            {
                throw new ArgumentNullException(
                    "--output",
                    "An output path must be specified using --output");
            }
        }
    }
}