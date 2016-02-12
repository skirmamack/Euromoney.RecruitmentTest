using System;
using System.Collections.Generic;
using ContentConsole.IO;
using ContentConsole.Repositories;

namespace ContentConsole.ContentProcessor
{
    public class ContentProcessorLauncher
    {
        private readonly IContentProcessor _contentProcessor;
        private readonly IWordsRepository _wordsRepository;
        private readonly ITextFileReader _textFileReader;
        private readonly IInputOutput _inputOutput;

        private string _content;
        private IList<string> _negativeWords;

        public ContentProcessorLauncher() : this(new ConsoleInputOutput())
        {
            
        }

        public ContentProcessorLauncher(IInputOutput inputOutput) : this(new ContentProcessor(), new WordsRepository(), new TextFileReader(), inputOutput )
        {
            
        }

        public ContentProcessorLauncher(IContentProcessor contentProcessor, IWordsRepository wordsRepository, ITextFileReader textFileReader, IInputOutput inputOutput)
        {
            _contentProcessor = contentProcessor;
            _wordsRepository = wordsRepository;
            _textFileReader = textFileReader;
            _inputOutput = inputOutput;
        }

        private void DisplayMenu()
        {
            _inputOutput.Clear();
            _inputOutput.WriteLine("Select mode:");
            _inputOutput.WriteLine("1) Display negative words count");
            _inputOutput.WriteLine("2) Filter negative words");
            _inputOutput.WriteLine("3) Display original text");
            _inputOutput.WriteLine("Any other - exit");
        }

        public void Run()
        {
            var run = true;
            while (run)
            {
                DisplayMenu();

                switch (_inputOutput.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        RunCommand(Command.DisplayNegativeCount);
                        break;

                    case ConsoleKey.D2:
                        RunCommand(Command.FilterNegative);
                        break;

                    case ConsoleKey.D3:
                        RunCommand(Command.DisplayOriginalContent);
                        break;

                    default:
                        run = false;
                        break;
                }
            }
        }

        private void RunCommand(Command command)
        {
            _content = EnsureContent(_content);
            _negativeWords = EnsureNegativeWords(_negativeWords);
            _inputOutput.Clear();
            _contentProcessor.Run(command, _content, _negativeWords);
        }

        private string EnsureContent(string content)
        {
            return content ?? _textFileReader.Read(Constants.DefaultContentFile);
        }

        private IList<string> EnsureNegativeWords(IList<string> negativeWords)
        {
            return negativeWords ?? _wordsRepository.GetNegativeWords(Constants.DefaultBannedWordsFile);
        }
    }
}
