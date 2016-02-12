using System.Collections.Generic;

namespace ContentConsole.ContentProcessor
{
    public interface IContentProcessor
    {
        ContentProcessorResult Run(Command command, string content, IList<string> negativeWords);
    }
}