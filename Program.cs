using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Diff_CP;

namespace DiffApp
{
    class Program
    {
        private static readonly TextComparer textComparer = new TextComparer();

        private static readonly string LeftFileStorage = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DiffApp", "leftfile.txt");

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }

            try
            {
                switch (args[0].ToLower())
                {
                    case "--set-left":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Error: No file specified for left side.");
                            return;
                        }
                        SetLeftFile(args[1]);
                        break;

                    case "--compare":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Error: No file specified for comparison.");
                            return;
                        }
                        CompareWithLeft(args[1]);
                        break;

                    case "--patch":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Error: No file specified for patch generation.");
                            return;
                        }
                        GeneratePatchWithLeft(args[1]);
                        break;

                    case "--diff":
                        if (args.Length < 3)
                        {
                            Console.WriteLine("Error: Need two files to compare.");
                            ShowUsage();
                            return;
                        }
                        ShowDiff(args[1], args[2]);
                        break;

                    case "--create-patch":
                        if (args.Length < 3)
                        {
                            Console.WriteLine("Error: Need two files to create patch.");
                            ShowUsage();
                            return;
                        }
                        string? outputPath = args.Length > 3 ? args[3] : null;
                        CreatePatchFile(args[1], args[2], outputPath);
                        break;

                    default:
                        ShowUsage();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void SetLeftFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found: {filePath}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            // Ensure directory exists
            var leftStorageDir = Path.GetDirectoryName(LeftFileStorage);
            if (leftStorageDir != null)
            {
                Directory.CreateDirectory(leftStorageDir);
            }

            // Store the path of the left file
            File.WriteAllText(LeftFileStorage, filePath);

            Console.WriteLine($"Left file set: {Path.GetFileName(filePath)}");
            Console.WriteLine("Now right-click another file and select 'Compare with Left'");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void CompareWithLeft(string rightFilePath)
        {
            if (!File.Exists(rightFilePath))
            {
                Console.WriteLine($"Error: Right file not found: {rightFilePath}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(LeftFileStorage))
            {
                Console.WriteLine("Error: No left file set. Please right-click a file and select 'Set as Diff Left' first.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            string leftFilePath = File.ReadAllText(LeftFileStorage).Trim();

            if (!File.Exists(leftFilePath))
            {
                Console.WriteLine($"Error: Previously selected left file no longer exists: {leftFilePath}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Comparing:");
            Console.WriteLine($"Left:  {leftFilePath}");
            Console.WriteLine($"Right: {rightFilePath}");
            Console.WriteLine(new string('=', 80));

            ShowDiff(leftFilePath, rightFilePath);
        }

        private static void GeneratePatchWithLeft(string rightFilePath)
        {
            if (!File.Exists(rightFilePath))
            {
                Console.WriteLine($"Error: Right file not found: {rightFilePath}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(LeftFileStorage))
            {
                Console.WriteLine("Error: No left file set. Please right-click a file and select 'Set as Diff Left' first.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            string leftFilePath = File.ReadAllText(LeftFileStorage).Trim();

            if (!File.Exists(leftFilePath))
            {
                Console.WriteLine($"Error: Previously selected left file no longer exists: {leftFilePath}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Generating patch file for:");
            Console.WriteLine($"Original: {leftFilePath}");
            Console.WriteLine($"Modified: {rightFilePath}");
            Console.WriteLine();

            CreatePatchFile(leftFilePath, rightFilePath, null);
        }

        private static void ShowDiff(string leftFile, string rightFile)
        {
            // Your existing diff logic goes here
            // This is a simple example - replace with your actual diff implementation

            string[] leftLines = File.ReadAllLines(leftFile);
            string[] rightLines = File.ReadAllLines(rightFile);

            var diff = textComparer.CompareTexts(
                leftLines.ToList(),
                rightLines.ToList()
            );

            Console.WriteLine("Diff:");
            Console.WriteLine();
            Console.WriteLine();

            foreach (var line in diff)
            {
                switch(line.Type)
                {
                    case TextOpType.Insert:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(line.ToString());
                        break;
                    case TextOpType.Delete:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(line.ToString());
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(line.ToString());
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Diff complete. Press any key to exit...");
            Console.ReadKey();
        }

        private static void CreatePatchFile(string originalFile, string modifiedFile, string? outputPath = null)
        {
            try
            {
                string[] originalLines = File.ReadAllLines(originalFile);
                string[] modifiedLines = File.ReadAllLines(modifiedFile);

                string patch = GenerateUnifiedDiff(originalFile, modifiedFile, originalLines, modifiedLines);

                if (string.IsNullOrEmpty(outputPath))
                {
                    string originalName = Path.GetFileNameWithoutExtension(originalFile);
                    string modifiedName = Path.GetFileNameWithoutExtension(modifiedFile);
                    outputPath = $"{originalName}_to_{modifiedName}.patch";
                }

                // Ensure we have full path
                if (!Path.IsPathRooted(outputPath))
                {
                    outputPath = Path.Combine(Environment.CurrentDirectory, outputPath);
                }

                File.WriteAllText(outputPath, patch, Encoding.UTF8);

                Console.WriteLine($"Patch file created: {outputPath}");
                Console.WriteLine($"File size: {new FileInfo(outputPath).Length} bytes");
                Console.WriteLine();
                Console.WriteLine("Patch preview:");
                Console.WriteLine(new string('=', 40));
                Console.WriteLine(patch);
                Console.WriteLine(new string('=', 40));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating patch file: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static string GenerateUnifiedDiff(string originalFile, string modifiedFile, string[] originalLines, string[] modifiedLines)
        {
            var sb = new StringBuilder();

            // File headers
            var originalTime = File.GetLastWriteTime(originalFile).ToString("yyyy-MM-dd HH:mm:ss.fff");
            var modifiedTime = File.GetLastWriteTime(modifiedFile).ToString("yyyy-MM-dd HH:mm:ss.fff");

            sb.AppendLine($"--- {Path.GetFileName(originalFile)}\t{originalTime}");
            sb.AppendLine($"+++ {Path.GetFileName(modifiedFile)}\t{modifiedTime}");
            sb.AppendLine($"@@ -1,{originalLines.Length} +1,{modifiedLines.Length} @@");

            var diff = textComparer.CompareTexts(
                originalLines.ToList(),
                modifiedLines.ToList()
            );

            foreach (var line in diff)
            {
                sb.AppendLine(line.ToString());
            }

            return sb.ToString();
        }

        private static void ShowUsage()
        {
            Console.WriteLine("DiffApp Usage:");
            Console.WriteLine("  --set-left <file>           Set file as left side for comparison");
            Console.WriteLine("  --compare <file>            Compare file with previously set left file");
            Console.WriteLine("  --patch <file>              Generate patch file comparing with left file");
            Console.WriteLine("  --diff <file1> <file2>      Compare two files directly");
            Console.WriteLine("  --create-patch <orig> <mod> [output.patch]  Create patch file");
            Console.WriteLine();
            Console.WriteLine("For context menu integration:");
            Console.WriteLine("1. Run the registry script to add context menu items");
            Console.WriteLine("2. Right-click a file and select 'Set as Diff Left'");
            Console.WriteLine("3. Right-click another file and select 'Compare with Left' or 'Generate Patch'");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  DiffApp.exe --create-patch old.txt new.txt changes.patch");
            Console.WriteLine("  DiffApp.exe --diff file1.cs file2.cs");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}