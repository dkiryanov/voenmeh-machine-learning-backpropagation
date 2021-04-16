using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IPlotService
    {
        void PlotTrainingCurve(IEnumerable<double> errors, int epochCount);

        void PlotTrainingAndValidationCurves(IEnumerable<double> errors, IEnumerable<double> validationErrors, int epochCount);
    }
}