using System.IO;

namespace ContentConsole.IO
{
    public class TextFileReader : ITextFileReader
    {
        public string Read(string filename)
        {
            try
            {  
                using (var sr = new StreamReader(filename))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
