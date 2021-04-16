using Deedle;

namespace BLL.Services.Interfaces
{
    public interface IFileService
    {
        string CreateFilePath(string fileName);

        Frame<int, string> GetDataFromFile(string fileName);
    }
}