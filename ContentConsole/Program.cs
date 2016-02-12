using ContentConsole.ContentProcessor;

namespace ContentConsole
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var launcher = new ContentProcessorLauncher();
            launcher.Run();
        }
    }

}
