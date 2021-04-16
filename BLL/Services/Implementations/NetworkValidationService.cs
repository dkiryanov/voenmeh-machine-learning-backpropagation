using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Accord.Neuro;
using BLL.Models;
using BLL.Services.Interfaces;
using Deedle;

namespace BLL.Services.Implementations
{
    public class NetworkValidationService : INetworkValidationService
    {
        //private static Dictionary<string, string> _symbols = new Dictionary<string, string>()
        //{
        //    {"1000", "8"},
        //    {"0100", "9"},
        //    {"0010", "Ы"},
        //    {"0001", "Р"}
        //};

        private readonly Dictionary<int, string> _symbolEncodings;

        public NetworkValidationService()
        {
            _symbolEncodings = new Dictionary<int, string>()
            {
                { 0, "1000" },
                { 1, "0100" },
                { 2, "0010" },
                { 3, "0001" }
            };
        }

        public double Validate(
            Frame<int, string> validation,
            ActivationNetwork network,
            string[] labelColumns,
            string[] featureColumns)
        {
            // prep feature and label arrays
            double[][] validationFeatures = validation.Columns[featureColumns].ToArray2D<double>().ToJagged();
            double[][] validationLabels = validation.Columns[labelColumns].ToArray2D<double>().ToJagged();

            List<double> result = new List<double>();

            for (int i = 0; i < validation.RowKeys.Count(); i++)
            {
                double[] feature = validationFeatures[i];
                double[] label = validationLabels[i];

                double[] predictions = network.Compute(feature);

                List<int> symbolEncoding = label.Select(v => (int)v).ToList();
                string symbol = String.Join("", symbolEncoding);
                int symbolIndex = _symbolEncodings.First(v => v.Value == symbol).Key;

                // calculate best prediction
                var best = Enumerable.Range(0, 4)
                    .Select(v => new { Digit = _symbolEncodings[v], Prediction = predictions[v] })
                    .OrderByDescending(v => v.Prediction)
                    .First();

                // count incorrect predictions
                if (best.Digit != symbol)
                {
                    double symbolPrediction = predictions[symbolIndex];
                    result.Add(1 - symbolPrediction);
                }
                else
                {
                    result.Add(1 - best.Prediction);
                }
            }

            return result.Average();
        }

        public PredictionInfoModel ValidateSingleFeature(ActivationNetwork network, double[] feature)
        {
            double[] predictions = network.Compute(feature);

            return Enumerable.Range(0, 4)
                .Select(v => new PredictionInfoModel
                {
                    Symbol = _symbolEncodings[v],
                    Probability = predictions[v]
                })
                .OrderByDescending(v => v.Probability)
                .First();
        }

        public IEnumerable<PredictionInfoModel> BulkValidation(
            Frame<int, string> validation,
            ActivationNetwork network,
            string[] labelColumns,
            string[] featureColumns)
        {
            
            double[][] validationFeatures = validation.Columns[featureColumns].ToArray2D<double>().ToJagged();
            double[][] validationLabels = validation.Columns[labelColumns].ToArray2D<double>().ToJagged();

            List<PredictionInfoModel> result = new List<PredictionInfoModel>(validation.RowKeys.Count());

            for (int i = 0; i < validation.RowKeys.Count(); i++)
            {
                double[] feature = validationFeatures[i];
                string expectedSymbol = GetSymbol(validationLabels[i]);

                PredictionInfoModel predictionResult = ValidateSingleFeature(network, feature);
                predictionResult.ExpectedSymbol = expectedSymbol;

                if (predictionResult.Symbol != expectedSymbol)
                {
                    predictionResult.IsCorrect = false;
                }

                predictionResult.IsCorrect = true;

                result.Add(predictionResult);
            }

            return result;
        }

        private static string GetSymbol(double[] label)
        {
            List<int> symbolEncoding = label.Select(v => (int)v).ToList();
            return string.Join("", symbolEncoding);
        }
    }
}
