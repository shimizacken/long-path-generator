using Ionic.Zip;
using System;
using System.IO;
using System.Linq;

namespace long_path
{
    internal class Program
    {
        private static int _folders;

        private static int _fileNameLength;
        private static string _newFileName;
        private static string _reallyLongDirectory;
        private static string _fullPath;
        private static int _folderNameSize = 240;

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Long Path Generator | runs with .NET 4.5.1\n\n");
                bool flag;

                do
                {
                    ValidateInputs();
                    _reallyLongDirectory = GetFolderPath();
                    GenerateFileName();
                    _fullPath = string.Format("{0}\\{1}", _reallyLongDirectory, _newFileName);
                    PrintSummary();
                    CreateFolder(_reallyLongDirectory);
                    CopyFileToScan();
                    AddFileToZip(_reallyLongDirectory);
                    Console.WriteLine("\n\nCreate another file? Y/N");
                    string text = Console.ReadLine();
                    flag = (text == string.Empty || text.ToLower() == "y");
                }
                while (flag);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n********* ERROR *********\n");
                Console.WriteLine(ex.Message);
                Console.WriteLine("\n********* Stack trace *********\n");
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }

        private static void CopyFileToScan()
        {
            string sourceFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sourcecode\\DOM_XSS.js");
            File.Copy(sourceFileName, _reallyLongDirectory + "\\" + _newFileName);
        }

        private static void GenerateFileName()
        {
            bool flag = _fileNameLength > 5;

            if (flag)
            {
                _fileNameLength -= 3;
            }

            _newFileName = RandomString(_fileNameLength).ToLower() + ".js";
        }

        private static void PrintSummary()
        {
            Console.WriteLine("\n{0}\\{1}{2}", _reallyLongDirectory, _newFileName, Environment.NewLine);
            Console.WriteLine("Summary:\n");
            Console.WriteLine("Directories length: {0}\nTotal File name length: {2}, '{1}'\n", _reallyLongDirectory.Length, _newFileName, _newFileName.Length);
            Console.WriteLine("-------------------");
            Console.WriteLine("Total: {0}\n\n", _fullPath.Length);
        }

        private static void CreateFolder(string reallyLongDirectory)
        {
            Directory.CreateDirectory(reallyLongDirectory);
            Console.WriteLine(string.Format("Creating {0} directories of {1} characters long", _folders, _fullPath.Length) + Environment.NewLine);
        }

        private static string GetFolderPath()
        {
            string text = "C:\\";
            Console.WriteLine("\nbuild long path string" + Environment.NewLine);
            for (int i = 0; i < _folders; i++)
            {
                text = text + RandomString(_folderNameSize) + "\\";
            }
            return text.TrimEnd(new char[]
            {
                '\\'
            });
        }

        private static void ValidateInputs()
        {
            Console.WriteLine(string.Format("Please enter folders number. Every folder {0} char long: (default 20)", _folderNameSize));

            int.TryParse(Console.ReadLine(), out _folders);
            bool flag = _folders == 0;

            if (flag)
            {
                _folders = 20;
            }

            Console.WriteLine("Please enter file name length: (default 10 chars)");
            int.TryParse(Console.ReadLine(), out _fileNameLength);
            bool flag2 = _fileNameLength == 0;

            if (flag2)
            {
                _fileNameLength = 10;
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            return new string((from s in Enumerable.Repeat<string>("abcdefghijklmno-pqrstuv_wxyz0123456789", length)
                               select s[random.Next(s.Length)]).ToArray<char>());
        }

        private static void AddFileToZip(string reallyLongDirectory)
        {
            string text = string.Format("c:\\long-path_folders {0}_filename length {1}.zip", _folders, _fileNameLength);

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(reallyLongDirectory, reallyLongDirectory);
                zipFile.Save(text);
                Console.WriteLine("zip file '" + text + "' saved successfully");
            }
        }

        private static void DeleteFolder(string reallyLongDirectory)
        {
            Directory.Delete(reallyLongDirectory);
        }
    }
}
