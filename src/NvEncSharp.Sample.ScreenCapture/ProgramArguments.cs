using System;
using System.Collections.Generic;

namespace Lennox.NvEncSharp.Sample.ScreenCapture
{
    internal class ProgramArguments
    {
        public string DisplayName { get; set; }
        public string OutputPath { get; set; }

        public ProgramArguments(IReadOnlyList<string> args)
        {
            for (var i = 0; i < args.Count; ++i)
            {
                string GetNextArgument()
                {
                    if (i + 1 > args.Count - 1)
                    {
                        throw new ArgumentNullException(
                            args[i], "Argument required.");
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
                    default:
                        throw new ArgumentOutOfRangeException(
                            args[i], "Unknown argument.");
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