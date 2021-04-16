using System.Collections.Generic;
using System.Linq;
using Accord.Controls;
using Accord.Statistics.Visualizations;
using BLL.Resources;
using BLL.Services.Interfaces;

namespace BLL.Services.Implementations
{
    public class PlotService : IPlotService
    {
        public void PlotTrainingCurve(IEnumerable<double> errors, int epochCount)
        {
            double[] x = Enumerable
                .Range(1, epochCount)
                .Select(v => (double)v)
                .ToArray();

            double[] y = errors.ToArray();

            Scatterplot plot = new Scatterplot(
                TrainingPlotResources.Title, 
                TrainingPlotResources.YAxisTitle, 
                TrainingPlotResources.XAxisTitle);
            plot.Compute(x, y);

            ScatterplotBox.Show(plot);
        }

        public void PlotTrainingAndValidationCurves(
            IEnumerable<double> errors, 
            IEnumerable<double> validationErrors, 
            int epochCount)
        {
            IEnumerable<double> tmp = Enumerable
                .Range(1, epochCount)
                .Select(v => (double)v)
                .ToList();

            double[] x = tmp.Concat(tmp).ToArray();
            double[] y = errors.Concat(validationErrors).ToArray();
            int[] z = Enumerable
                .Repeat(1, epochCount)
                .Concat(Enumerable.Repeat(2, epochCount))
                .ToArray();

            Scatterplot plot = new Scatterplot(
                TrainingAndValidationPlotResources.Title, 
                TrainingAndValidationPlotResources.XAxisTitle,
                TrainingAndValidationPlotResources.YAxisTitle);

            plot.Compute(x, y, z);

            ScatterplotBox.Show(plot);
        }
    }
}
