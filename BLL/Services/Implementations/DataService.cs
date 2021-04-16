using Accord.IO;
using Accord.Neuro;
using BLL.Services.Interfaces;
using Deedle;

namespace BLL.Services.Implementations
{
    public class DataService : IDataService
    {
        private const string ValidationDataFileName = "validation.csv";
        private const string TrainigDataFileName = "training.csv";
        private const string NetworkStateFileName = "trained_network.state";

        private readonly IFileService _fileService;

        public DataService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public Frame<int, string> GetValidationData()
        {
            return _fileService.GetDataFromFile(ValidationDataFileName);
        }

        public Frame<int, string> GetTrainingData()
        {
            return _fileService.GetDataFromFile(TrainigDataFileName);
        }

        public void SaveNetworkState(ActivationNetwork network)
        {
            string fileName = _fileService.CreateFilePath(NetworkStateFileName);
            Serializer.Save(network, fileName);
        }
    }
}