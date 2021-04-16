using Accord.Neuro;
using Deedle;

namespace BLL.Services.Interfaces
{
    public interface IDataService
    {
        Frame<int, string> GetValidationData();

        Frame<int, string> GetTrainingData();

        void SaveNetworkState(ActivationNetwork network);
    }
}