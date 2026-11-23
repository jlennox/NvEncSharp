using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lennox.NvEncSharp.Test.CodeGeneration;

namespace Lennox.NvEncSharp.Test
{
    [TestClass]
    public sealed class CudaEntryPointTests
    {
        private static readonly Regex _conditional = new Regex(@"^\s*#\s*(if|ifdef|ifndef|elif|else|endif)\b(.*)$");
        private static readonly Regex _versionComparison = new Regex(@"__CUDA_API_VERSION\s*(>=|<)\s*(\d+)");
        private static readonly Regex _versionedDefine = new Regex(
            @"^\s*#\s*define\s+(cu\w+)\s+(?:__CUDA_API_PT(?:SZ|DS)\()?(cu\w+_v\d+)\)?\s*$");

        private sealed class Conditional
        {
            public string Condition;
            public bool InAlternateBranch;
        }

        private static bool IsActive(IEnumerable<Conditional> conditionals, string define)
        {
            foreach (var conditional in conditionals)
            {
                var comparisons = _versionComparison.Matches(conditional.Condition).Cast<Match>().ToArray();
                if (comparisons.Length == 0) continue;

                if (conditional.InAlternateBranch)
                {
                    throw new NotSupportedException($"\"{define}\" is in the alternate branch of \"{conditional.Condition}\".");
                }

                foreach (var comparison in comparisons)
                {
                    var bound = int.Parse(comparison.Groups[2].Value);
                    var holds = comparison.Groups[1].Value == ">="
                        ? LibCuda.ApiVerison >= bound
                        : LibCuda.ApiVerison < bound;
                    if (!holds) return false;
                }
            }

            return true;
        }

        // cuda.h renames functions whose ABI changed, ie, "#define cuMemAlloc cuMemAlloc_v2".
        private static Dictionary<string, string> VersionedEntryPoints()
        {
            var versioned = new Dictionary<string, string>();
            var conditionals = new Stack<Conditional>();

            foreach (var line in LoadCudaHeader().Split('\n'))
            {
                var conditional = _conditional.Match(line);
                if (conditional.Success)
                {
                    switch (conditional.Groups[1].Value)
                    {
                        case "if":
                        case "ifdef":
                        case "ifndef":
                            conditionals.Push(new Conditional { Condition = conditional.Groups[2].Value });
                            break;
                        case "elif":
                        case "else":
                            conditionals.Peek().InAlternateBranch = true;
                            break;
                        case "endif":
                            conditionals.Pop();
                            break;
                    }

                    continue;
                }

                var define = _versionedDefine.Match(line.TrimEnd('\r'));
                if (!define.Success) continue;
                if (!IsActive(conditionals, define.Groups[1].Value)) continue;

                versioned.Add(define.Groups[1].Value, define.Groups[2].Value);
            }

            Assert.AreEqual(0, conditionals.Count);
            return versioned;
        }

        [TestMethod]
        public void VersionedFunctionsAreBoundToVersionedEntryPoints()
        {
            var versioned = VersionedEntryPoints();
            Assert.AreEqual("cuMemAllocHost_v2", versioned["cuMemAllocHost"]);
            Assert.AreEqual("cuTexRefSetAddress2D_v3", versioned["cuTexRefSetAddress2D"]);

            var entryPoints = typeof(LibCuda)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(method => method.GetCustomAttribute<DllImportAttribute>())
                .Where(attribute => attribute != null)
                .Select(attribute => attribute.EntryPoint)
                .ToArray();
            CollectionAssert.Contains(entryPoints, "cuMemAllocHost_v2");

            var unversioned = entryPoints
                .Where(versioned.ContainsKey)
                .Select(entryPoint => $"{entryPoint} should be {versioned[entryPoint]}")
                .OrderBy(message => message)
                .ToArray();

            Assert.AreEqual(0, unversioned.Length, string.Join(Environment.NewLine, unversioned));
        }
    }
}
