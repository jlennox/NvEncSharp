using System;

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal class ProgramArguments
    {
        public string InputPath { get; set; }

        public ProgramArguments(string[] args)
        {
            for (var i = 0; i < args.Length; ++i)
            {
                string GetNextArgument()
                {
                    if (i + 1 > args.Length - 1)
                    {
                        throw new ArgumentNullException(
                            args[i], "Argument required.");
                    }

                    return args[++i];
                }

                switch (args[i])
                {
                    case "--input":
                    case "-i":
                        InputPath = GetNextArgument();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            args[i], "Unknown argument.");
                }
            }

            if (InputPath == null)
            {
                throw new ArgumentNullException(
                    "--input",
                    "An input path must be specified using --input");
            }
        }
    }
}