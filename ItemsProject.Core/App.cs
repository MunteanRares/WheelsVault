using ItemsProject.Core.Data;
using ItemsProject.Core.Databases;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace ItemsProject.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            // Interfaces + Implementations
            Mvx.IoCProvider.RegisterType<ISqlDataAccess, SqlDataAccess>();
            Mvx.IoCProvider.RegisterType<IDatabaseData, SqlData>();
            Mvx.IoCProvider.RegisterType<IDataService, DataService>();
            Mvx.IoCProvider.RegisterType<IFolderDataService, FolderDataService>();
            Mvx.IoCProvider.RegisterType<IMessageBoxDataService, MessageBoxDataService>();
            Mvx.IoCProvider.RegisterType<IItemDataService, ItemDataService>();

            // ViewModels
            Mvx.IoCProvider.RegisterType<BaseViewModel>();
            Mvx.IoCProvider.RegisterType<AddFolderViewModel>();
            Mvx.IoCProvider.RegisterType<CustomMessageBoxViewModel>();
            Mvx.IoCProvider.RegisterType<AddItemViewModel>();

            // Config
            Mvx.IoCProvider.RegisterSingleton(configuration);

            RegisterAppStart<BaseViewModel>();
        }
    }
}
