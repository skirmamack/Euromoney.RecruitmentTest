using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentConsole.IO;

namespace ContentConsole.ContentProcessor
{
    public class ContentProcessor : IContentProcessor
    {
        private readonly IInputOutput _inputOutput;

        public ContentProcessor() : this(new ConsoleInputOutput())
        {
        }

        public ContentProcessor(IInputOutput inputOutput)
        {
            _inputOutput = inputOutput;
        }

        public ContentProcessorResult Run(Command command, string content, IList<string> negativeWords)
        {
            content = content ?? "";
            negativeWords = negativeWords ?? new List<string>();

            var matches = GetNegativeWordMatches(content, negativeWords);

            var result = new ContentProcessorResult
            {
                NegativeWordsCount = matches.Count,
                Content = command.Equals(Command.FilterNegative) ? MaskNegativeWords(content, matches) : content
            };

            return DisplayResult(result.Content, result, command);
        }


        private ContentProcessorResult DisplayResult(string content, ContentProcessorResult result, Command command)
        {
            if (command.Equals(Command.DisplayNegativeCount))
            {
                _inputOutput.WriteLine("Scanned the text:");
            }

            _inputOutput.WriteLine(content);

            if (command.Equals(Command.DisplayNegativeCount) || command.Equals(Command.DisplayOriginalContent))
            {
                _inputOutput.WriteLine("Total Number of negative words: " + result.NegativeWordsCount);
            }

            _inputOutput.WriteLine("Press ANY key to exit.");
            _inputOutput.ReadKey();
            return result;
        }

        private static string MaskNegativeWords(string content, ICollection matches)
        {
            if (matches.Count == 0) return content;

            var filteredContent = new StringBuilder(content);
            foreach (Match match in matches)
            {
                MaskSingleMatch(filteredContent, match);
            }
            return filteredContent.ToString();
        }

        private static void MaskSingleMatch(StringBuilder filteredContent, Match match)
        {
            for (var maskIndex = match.Index + 1; maskIndex < match.Index + match.Length - 1; maskIndex++)
            {
                filteredContent[maskIndex] = '#';
            }
        }

        private static ICollection GetNegativeWordMatches(string content, IList<string> negativeWords)
        {
            if (!negativeWords.Any())
            {
                return new Match[] {};
            }

            var negativeWordsAggregated = negativeWords
                .Select(w => "\\b" + w + "\\b")
                .Aggregate((a, b) => a + "|" + b);

            var regEx = new Regex(negativeWordsAggregated, RegexOptions.IgnoreCase);

            return regEx.Matches(content);
        }
    }
}
