using System;
using System.Collections.Generic;
using System.Text;
using ContentConsole.IO;

namespace ContentConsole.Test.Unit.Stubs
{
    public class InputOutputStub : IInputOutput
    {
        public StringBuilder Output { get; set; }

        public Queue<ConsoleKeyInfo> Keys { get; set; }

        private static ConsoleKeyInfo EnterKey
        {
            get { return new ConsoleKeyInfo(' ', ConsoleKey.Enter, false, false, false); }
        }

        public InputOutputStub()
        {
            Output = new StringBuilder();
            Keys = new Queue<ConsoleKeyInfo>();
        }

        public void WriteLine(string line)
        {
            Output.AppendLine(line);
        }

        public void AddKeys(params ConsoleKey[] keys)
        {
            foreach (var key in keys)
            {
                AddKey(key);
            }
        }

        private void AddKey(ConsoleKey key)
        {
            Keys.Enqueue(new ConsoleKeyInfo(' ', key, false, false, false));            
        }

        public ConsoleKeyInfo ReadKey()
        {
            Output.AppendLine("<READ KEY>");
            var key = Keys.Count == 0 ? EnterKey : Keys.Dequeue();
            Output.AppendLine(key.Key.ToString());
            return key;
        }

        public void Clear()
        {
            Output.AppendLine("<CLEAR>");
        }

        public string GetOutput()
        {
            return Output.ToString();
        }
    }
}