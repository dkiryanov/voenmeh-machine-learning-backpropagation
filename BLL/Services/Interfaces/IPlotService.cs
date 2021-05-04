using System.Collections.Generic;

namespace BLL.Services.Interfaces
{
    public interface IPlotService
    {
        void PlotTrainingCurve(IEnumerable<double> errors, int epochCount);

        void PlotValidationCurve(IEnumerable<double> errors, int epochCount);
    }
}