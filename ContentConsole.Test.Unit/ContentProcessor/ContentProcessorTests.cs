using NUnit.Framework;
using ContentConsole.ContentProcessor;
using ContentConsole.Test.Unit.Stubs;

namespace ContentConsole.Test.Unit.ContentProcessor
{
    [TestFixture]
    public class ContentProcessorTests
    {
        private InputOutputStub _inputOutput;
        private ContentConsole.ContentProcessor.ContentProcessor _commandProcessor;

        [SetUp]
        public void SetUp()
        {
            _inputOutput = new InputOutputStub();
            _commandProcessor = new ContentConsole.ContentProcessor.ContentProcessor(_inputOutput);
        }

        [TestCase("bad", 1)]
        [TestCase("(bad", 1)]
        [TestCase("bad)", 1)]
        [TestCase("(bad)", 1)]
        [TestCase("happy bad happy", 1)]
        [TestCase("bad Bad bAd (bad) OK OK sad. happy baD", 6)]
        public void should_find_correct_number_of_negative_words(string content, int expectedBadWordsCount)
        {
            //given
            var negativeWords = new[] {"bad", "sad"};
            const Command command = Command.DisplayNegativeCount;

            //when
            var result = _commandProcessor.Run(command, content, negativeWords);

            //then
            Assert.That(result.NegativeWordsCount, Is.EqualTo(expectedBadWordsCount));
        }

        [TestCase("notbad")]
        [TestCase("badoer")]
        public void should_not_find_partial_matches(string content)
        {
            //given
            var negativeWords = new[] { "bad" };
            const Command command = Command.DisplayNegativeCount;

            //when
            var result = _commandProcessor.Run(command, content, negativeWords);

            //then
            Assert.That(result.NegativeWordsCount, Is.EqualTo(0));            
        }

        [TestCase("Horrible", "horrible", "H######e")]
        [TestCase("horrible", "horrible", "h######e")]
        [TestCase("Horrible horrible", "horrible", "H######e h######e")]
        [TestCase("nice terrible good", "terrible", "nice t######e good")]
        public void should_replace_middle_characters_with_hashes(string content, string negativeWord, string expectedResultContent)
        {
            //given
            var negativeWords = new[] { negativeWord };
            const Command command = Command.FilterNegative;

            //when
            var result = _commandProcessor.Run(command, content, negativeWords);

            //then
            Assert.That(result.Content, Is.EqualTo(expectedResultContent));
        }

        [TestCase("f", "f")]
        [TestCase("fe", "fe")]
        public void should_not_filter_if_less_than_three_characters(string content, string negativeWord)
        {
            //given
            var negativeWords = new[] { negativeWord };
            const Command command = Command.FilterNegative;

            //when
            var result = _commandProcessor.Run(command, content, negativeWords);

            //then
            Assert.That(result.Content, Is.EqualTo(content));
        }

        [Test]
        public void for_user_should_do_correct_output()
        {
            //given
            const string content = "The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.";
            var negativeWords = new[] { "bad", "horrible" };
            const Command command = Command.DisplayNegativeCount;

            const string expectedOutput = @"Scanned the text:
The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.
Total Number of negative words: 2
Press ANY key to exit.
<READ KEY>
";

            //when
            _commandProcessor.Run(command, content, negativeWords);

            //then
            var output = _inputOutput.GetOutput();
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void for_reader_should_display_filtered_content()
        {
            //given
            const string content = "The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.";
            var negativeWords = new[] { "bad", "horrible" };
            const Command command = Command.FilterNegative;

            const string expectedOutput = @"The weather in Manchester in winter is b#d. It rains all the time - it must be h######e for people visiting.
Press ANY key to exit.
<READ KEY>
";

            //when
            _commandProcessor.Run(command, content, negativeWords);

            //then
            var output = _inputOutput.GetOutput();
            Assert.That(output, Is.EqualTo(expectedOutput));
        }

        [Test]
        public void for_curator_should_display_original_content()
        {
            //given
            const string content = "The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.";
            var negativeWords = new[] { "bad", "horrible" };
            const Command command = Command.DisplayOriginalContent;

            const string expectedOutput = @"The weather in Manchester in winter is bad. It rains all the time - it must be horrible for people visiting.
Total Number of negative words: 2
Press ANY key to exit.
<READ KEY>
";

            //when
            _commandProcessor.Run(command, content, negativeWords);

            //then
            var output = _inputOutput.GetOutput();
            Assert.That(output, Is.EqualTo(expectedOutput));
        }
    }
}
