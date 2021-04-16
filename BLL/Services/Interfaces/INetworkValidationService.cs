using System.Collections.Generic;
using Accord.Neuro;
using BLL.Models;
using Deedle;

namespace BLL.Services.Interfaces
{
    public interface INetworkValidationService
    {
        double Validate(
            Frame<int, string> validation, 
            ActivationNetwork network, string[] labelColumns, 
            string[] featureColumns);

        IEnumerable<PredictionInfoModel> BulkValidation(
            Frame<int, string> validation,
            ActivationNetwork network,
            string[] labelColumns,
            string[] featureColumns);

        PredictionInfoModel ValidateSingleFeature(ActivationNetwork network, double[] feature);
    }
}