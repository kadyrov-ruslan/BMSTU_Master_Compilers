using System.IO;

namespace Lab_1_v5
{
    public class FileIoHandler
    {
        public static string[] ReadTxtFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}
