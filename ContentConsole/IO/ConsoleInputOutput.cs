using System;

namespace ContentConsole.IO
{
    public class ConsoleInputOutput : IInputOutput
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
