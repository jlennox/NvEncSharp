using System;
using System.Collections.Generic;
using System.IO;

namespace Lennox.NvEncSharp.Sample.VideoDecode
{
    internal class ProgramArguments
    {
        public string InputPath { get; set; }
        public bool UseHostMemory { get; set; } = true;
        public string BitmapPath { get; set; }
        public bool WriteBitmap { get; set; }

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
                    case "--input":
                    case "-i":
                        InputPath = GetNextArgument();
                        break;
                    case "--host-memory":
                        UseHostMemory = true;
                        break;
                    case "--bitmap":
                        BitmapPath = GetNextArgument();
                        WriteBitmap = true;
                        if (!Directory.Exists(BitmapPath))
                        {
                            throw new ArgumentOutOfRangeException(
                                args[i],
                                "Directory does not exist. Argument must be an existing directory.");
                        }
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