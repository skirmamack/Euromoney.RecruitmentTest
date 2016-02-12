using System;
using ContentConsole.ContentProcessor;
using ContentConsole.IO;
using ContentConsole.Repositories;
using ContentConsole.Test.Unit.Stubs;
using Moq;
using NUnit.Framework;

namespace ContentConsole.Test.Unit.ContentProcessor
{
    public class ContentProcessorLauncherTests
    {
        private Mock<IContentProcessor> _contentProcessorMock;
        private Mock<IWordsRepository> _wordsRepositoryMock;
        private Mock<ITextFileReader> _textFileReaderMock;
        private InputOutputStub _inputOutputStub;
        private ContentProcessorLauncher _launcher;

        private const string SampleContent = "Some content";
        private readonly string[] _negativeWords = {"bad", "worst"};

        [SetUp]
        public void SetUp()
        {
            _contentProcessorMock = new Mock<IContentProcessor>();
            _wordsRepositoryMock = new Mock<IWordsRepository>();
            _textFileReaderMock = new Mock<ITextFileReader>();
            _inputOutputStub = new InputOutputStub();
            _launcher = new ContentProcessorLauncher(_contentProcessorMock.Object, _wordsRepositoryMock.Object, _textFileReaderMock.Object, _inputOutputStub);

            SetUpContent(SampleContent);
            SetUpNegativeWords(_negativeWords);
        }

        [Test]
        public void should_exit()
        {
            _inputOutputStub.AddKeys(ConsoleKey.Enter);

            _launcher.Run();

            VerifyCommandCalled(Command.DisplayNegativeCount, 0);
            VerifyCommandCalled(Command.FilterNegative, 0);
            VerifyCommandCalled(Command.DisplayOriginalContent, 0);
        }

        [Test]
        public void should_call_display_negative_words()
        {
            _inputOutputStub.AddKeys(ConsoleKey.D1, ConsoleKey.Enter);

            _launcher.Run();

            VerifyCommandCalled(Command.DisplayNegativeCount, 1);
            VerifyCommandCalled(Command.FilterNegative, 0);
            VerifyCommandCalled(Command.DisplayOriginalContent, 0);
        }

        [Test]
        public void should_call_filter_negative_words()
        {
            _inputOutputStub.AddKeys(ConsoleKey.D2, ConsoleKey.Enter);

            _launcher.Run();

            VerifyCommandCalled(Command.DisplayNegativeCount, 0);
            VerifyCommandCalled(Command.FilterNegative, 1);
            VerifyCommandCalled(Command.DisplayOriginalContent, 0);
        }

        [Test]
        public void should_call_display_original()
        {
            _inputOutputStub.AddKeys(ConsoleKey.D3, ConsoleKey.Enter);

            _launcher.Run();

            VerifyCommandCalled(Command.DisplayNegativeCount, 0);
            VerifyCommandCalled(Command.FilterNegative, 0);
            VerifyCommandCalled(Command.DisplayOriginalContent, 1);
        }

        [Test]
        public void should_call_all_correct_number_of_times()
        {
            _inputOutputStub.AddKeys(
                ConsoleKey.D1,
                ConsoleKey.D2, ConsoleKey.D2,
                ConsoleKey.D3, ConsoleKey.D3, ConsoleKey.D3,
                ConsoleKey.Enter);


            _launcher.Run();

            VerifyCommandCalled(Command.DisplayNegativeCount, 1);
            VerifyCommandCalled(Command.FilterNegative, 2);
            VerifyCommandCalled(Command.DisplayOriginalContent, 3);
        }

        [Test]
        public void should_display_menu()
        {
            _inputOutputStub.AddKeys(ConsoleKey.Enter);

            _launcher.Run();

            const string expectedOutput = @"<CLEAR>
Select mode:
1) Display negative words count
2) Filter negative words
3) Display original text
Any other - exit
<READ KEY>
Enter
";

            Assert.That(_inputOutputStub.GetOutput(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void should_return_to_menu_after_command()
        {
            _inputOutputStub.AddKeys(ConsoleKey.D1, ConsoleKey.Enter);

            _launcher.Run();

            const string expectedOutput = @"<CLEAR>
Select mode:
1) Display negative words count
2) Filter negative words
3) Display original text
Any other - exit
<READ KEY>
D1
<CLEAR>
<CLEAR>
Select mode:
1) Display negative words count
2) Filter negative words
3) Display original text
Any other - exit
<READ KEY>
Enter
";

            Assert.That(_inputOutputStub.GetOutput(), Is.EqualTo(expectedOutput));
        }

        #region Helpers

        private void SetUpContent(string content)
        {
            _textFileReaderMock
                .Setup(c => c.Read(It.IsAny<string>()))
                .Returns(content);
        }

        private void SetUpNegativeWords(string[] negativeWords)
        {
            _wordsRepositoryMock
                .Setup(r => r.GetNegativeWords(It.IsAny<string>()))
                .Returns(negativeWords);
        }

        private void VerifyCommandCalled(Command command, int times)
        {
            _contentProcessorMock
                .Verify(p => p.Run(command, SampleContent, _negativeWords), Times.Exactly(times));
        }

        #endregion
    }
}
