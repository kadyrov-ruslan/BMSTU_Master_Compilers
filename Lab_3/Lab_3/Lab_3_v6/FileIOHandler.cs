using System;
using System.IO;

namespace Lab_3_v6
{
    public class FileIoHandler
    {
        public static string[] ReadTxtFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public static bool WriteTxtFile(Grammar grammar)
        {
            throw new NotImplementedException();
        }
    }
}
