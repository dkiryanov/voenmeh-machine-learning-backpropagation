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

        public void PlotValidationCurve(IEnumerable<double> errors, int epochCount)
        {
            double[] x = Enumerable
                .Range(1, epochCount)
                .Select(v => (double)v)
                .ToArray();

            double[] y = errors.ToArray();

            Scatterplot plot = new Scatterplot(
                "График изменения квадратичной ошибки тестирования",
                TrainingPlotResources.YAxisTitle,
                "Ошибки тестирования");
            plot.Compute(x, y);

            ScatterplotBox.Show(plot);
        }
    }
}
