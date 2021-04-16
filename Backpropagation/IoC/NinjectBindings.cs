using BLL.Services.Implementations;
using BLL.Services.Interfaces;
using Ninject.Modules;

namespace Backpropagation.IoC
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataService>().To<DataService>();
            Bind<IFileService>().To<FileService>();
            Bind<IPlotService>().To<PlotService>();
            Bind<INetworkValidationService>().To<NetworkValidationService>();
        }
    }
}