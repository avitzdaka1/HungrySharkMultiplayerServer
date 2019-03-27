using System.IO;

namespace ServerApp
{
    public class Logger : LogBase
    {
        private string filePath = "log.txt";

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
