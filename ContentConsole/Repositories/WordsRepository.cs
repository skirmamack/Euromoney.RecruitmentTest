using System.Linq;
using ContentConsole.IO;

namespace ContentConsole.Repositories
{
    public class WordsRepository : IWordsRepository
    {
        private readonly ITextFileReader _textFileReader;

        public WordsRepository() : this(new TextFileReader())
        {
            
        }

        public WordsRepository(ITextFileReader textFileReader)
        {
            _textFileReader = textFileReader;
        }

        public string[] GetNegativeWords(string filename)
        {
            var content = _textFileReader.Read(filename);
            if (content == null) return null;

            return content
                .Split('\n')
                .Select(line => line.Replace("\r", ""))
                .Where(line => line.Length > 0)
                .ToArray();
        }
    }
}
