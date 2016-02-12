namespace ContentConsole.Repositories
{
    public interface IWordsRepository
    {
        string[] GetNegativeWords(string filename);
    }
}