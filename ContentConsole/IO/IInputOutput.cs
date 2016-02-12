using System;

namespace ContentConsole.IO
{
    public interface IInputOutput
    {
        void WriteLine(string line);
        ConsoleKeyInfo ReadKey();
        void Clear();
    }
}