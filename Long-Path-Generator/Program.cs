using Ionic.Zip;
using System;
using System.IO;
using System.Linq;

namespace long_path
{
    class Program
    {
        #region Fields

        static int _folders;
        static int _fileNameLength;
        static string _newFileName;
        static string _reallyLongDirectory;
        static string _fullPath;
        static int _folderNameSize = 240;

        #endregion

        #region API

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Long Path Generator | runs with .NET 4.5.1\n\n");

                Start:
                ValidateInputs();
                _reallyLongDirectory = GetFolderPath();
                GenerateFileName();
                _fullPath = string.Format("{0}\\{1}", _reallyLongDirectory, _newFileName);
                PrintSummary();
                CreateFolder(_reallyLongDirectory);
                //CopyFileToScan(Path.GetPathRoot(_reallyLongDirectory), @"sourcecode\DOM_XSS.js");
                CopyFileToScan(_reallyLongDirectory, @"sourcecode\DOM_XSS.js");
                AddFileToZip(_reallyLongDirectory);

                Console.WriteLine("\n\nCreate another file? Y/N");
                string answer = Console.ReadLine();

                if (answer == string.Empty || answer.ToLower() == "y")
                {
                    goto Start;
                }
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

        #endregion

        #region Private methods

        private static void CopyFileToScan(string dir, string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            File.Copy(path, dir + "\\" + _newFileName);
        }

        private static void GenerateFileName()
        {
            if (_fileNameLength > 5)
            {
                _fileNameLength = _fileNameLength - 3;
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

            Console.WriteLine($"Creating {_folders} directories of {_fullPath.Length} characters long" + Environment.NewLine);
        }

        private static string GetFolderPath()
        {
            string reallyLongDirectory = "C:\\";

            Console.WriteLine("\nbuild long path string" + Environment.NewLine);

            for (int i = 0; i < _folders; i++)
            {
                reallyLongDirectory += RandomString(_folderNameSize) + "\\";
            }

            reallyLongDirectory = reallyLongDirectory.TrimEnd('\\');

            return reallyLongDirectory;
        }

        private static void ValidateInputs()
        {
            Console.WriteLine($"Please enter folders number. Every folder {_folderNameSize} char long: (default 20)");

            int.TryParse(Console.ReadLine(), out _folders);

            if (_folders == 0)
            {
                _folders = 20;
            }

            Console.WriteLine("Please enter file name length: (default 10 chars)");

            int.TryParse(Console.ReadLine(), out _fileNameLength);

            if (_fileNameLength == 0)
            {
                _fileNameLength = 10;
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmno-pqrstuv_wxyz0123456789";

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static void AddFileToZip(string reallyLongDirectory)
        {
            string zipFileName = string.Format("c:\\long-path_folders {0}_filename length {1}.zip", _folders, _fileNameLength);

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(reallyLongDirectory, reallyLongDirectory);
                zip.Save(zipFileName);

                Console.WriteLine("zip file '" + zipFileName + "' saved successfully");
            }
        }

        private static void DeleteFolder(string reallyLongDirectory)
        {
            Directory.Delete(reallyLongDirectory);
        }

        #endregion
    }
}