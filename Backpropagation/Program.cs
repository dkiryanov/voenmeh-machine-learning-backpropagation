using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Backpropagation.IoC;
using BLL.Models;
using BLL.Services.Interfaces;
using Deedle;

namespace Backpropagation
{
    class Program
    {
        private const int EpochCount = 500;

        private static readonly string[] FeatureColumns = Enumerable.Range(5, 35).Select(v => $"Column{v}").ToArray();
        private static readonly string[] LabelColumns = Enumerable.Range(1, 4).Select(v => $"Column{v}").ToArray();

        private static readonly IDataService DataService = DependencyResolver.Resolve<IDataService>();
        private static readonly INetworkValidationService NetworkValidationService = DependencyResolver.Resolve<INetworkValidationService>();
        private static readonly IPlotService PlotService = DependencyResolver.Resolve<IPlotService>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(@"Загрузка данных для обучения и проверки...");
            Frame<int, string> training = DataService.GetTrainingData();
            Frame<int, string> validation = DataService.GetValidationData();

            double[][] features = training.Columns[FeatureColumns].ToArray2D<double>().ToJagged();
            double[][] labels = training.Columns[LabelColumns].ToArray2D<double>().ToJagged();

            // build a neural network
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), 35, 100, 100, 4);

            BackPropagationLearning learner = new BackPropagationLearning(network)
            {
                LearningRate = 0.05
            };
            new GaussianWeights(network, 0.1).Randomize();

            // train the neural network
            List<double> errors = new List<double>();
            List<double> validationErrors = new List<double>();

            for (int i = 0; i < EpochCount; i++)
            {
                double error = learner.RunEpoch(features, labels) / labels.GetLength(0);
                errors.Add(error);

                // validate network while we are training
                double validationError = NetworkValidationService
                    .Validate(validation, 
                                network,
                                LabelColumns, 
                                FeatureColumns);
                validationErrors.Add(validationError);

                Console.WriteLine($"Эпоха: {i}, Ошибка: {error}");
            }

            PlotService.PlotTrainingAndValidationCurves(errors, validationErrors, EpochCount);
            PlotService.PlotTrainingCurve(errors, EpochCount);

            PrintValidationReport(network, NetworkValidationService, training);

            Console.Write("Сохраняем состояние обученной сети на диск... ");
            DataService.SaveNetworkState(network);

            Console.ReadKey();
        }

        private static void PrintValidationReport(ActivationNetwork network, 
            INetworkValidationService networkValidationService,
            Frame<int, string> validation)
        {
            Console.WriteLine($"Тестирование нейронной сети на {validation.Rows.KeyCount} записях...");

            IEnumerable<PredictionInfoModel> validationResult = networkValidationService
                .BulkValidation(validation, network, LabelColumns, FeatureColumns);

            int numMistakes = 0;

            foreach (PredictionInfoModel prediction in validationResult)
            {
                if (!prediction.IsCorrect.Value)
                {
                    Console.WriteLine($@"ОШИБКА! {prediction.ExpectedSymbol}: -> {prediction.ExpectedSymbol} = {prediction.Symbol} ({100 * prediction.Probability:0.00}%)");
                    numMistakes++;
                }
                else
                {
                    Console.WriteLine($@"ВЕРНО. {prediction.ExpectedSymbol}: -> {prediction.ExpectedSymbol} = {prediction.Symbol} ({100 * prediction.Probability:0.00}%)");
                }
            }

            // the accuracy
            double accuracy = 100.0 * (validation.Rows.KeyCount - numMistakes) / validation.Rows.KeyCount;
            Console.WriteLine($@"Количество ошибок: {numMistakes}, Точность распознавания: {accuracy:0.00}%");
        }
    }
}