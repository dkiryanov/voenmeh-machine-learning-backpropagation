using System;
using System.IO;
using BLL.Services.Interfaces;
using Deedle;

namespace BLL.Services.Implementations
{
    public class FileService : IFileService
    {
        public Frame<int, string> GetDataFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return Frame.ReadCsv(
                Path.Combine(GetBasePath(), fileName),
                separators: ",",
                hasHeaders: false
            );
        }

        public string CreateFilePath(string fileName)
        {
            return Path.Combine(GetBasePath(), fileName);
        }

        private string GetBasePath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
            path = Path.Combine(path, "..");

            return path;
        }
    }
}