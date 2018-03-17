using System.IO;

namespace ServerApp
{
    public class Logger : LogBase
    {
        private string filePath = "/home/maks/log/log.txt";

        public override void Log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
        }
    }
}