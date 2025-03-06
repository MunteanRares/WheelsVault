using ItemsProject.Core.Data;
using ItemsProject.Core.Databases;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Nito.AsyncEx;
using WikiHotWheelsWebScraper.Services;

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

            Mvx.IoCProvider.RegisterSingleton(configuration);

            // Interfaces + Implementations
            string dbChoice = configuration.GetValue<string>("DatabaseChoice").ToLower();
            if (dbChoice == "sqlite")
            {
                Mvx.IoCProvider.RegisterType<IDatabaseData, SqliteData>();
            }
            else if (dbChoice == "sqlserver")
            {
                Mvx.IoCProvider.RegisterType<IDatabaseData, SqlData>();
            }

            Mvx.IoCProvider.RegisterType<ISqlDataAccess, SqlDataAccess>();
            Mvx.IoCProvider.RegisterType<ISqliteDataAccess, SqliteDataAccess>();
            Mvx.IoCProvider.RegisterType<IScrapeHotWheelsWiki, ScrapeHotWheelsWiki>();


            Mvx.IoCProvider.RegisterType<IDataService, DataService>();
            Mvx.IoCProvider.RegisterType<IFolderDataService, FolderDataService>();
            Mvx.IoCProvider.RegisterType<IMessageBoxDataService, MessageBoxDataService>();
            Mvx.IoCProvider.RegisterType<IItemDataService, ItemDataService>();


            // ViewModels
            Mvx.IoCProvider.RegisterType<BaseViewModel>();
            Mvx.IoCProvider.RegisterType<AddFolderViewModel>();
            Mvx.IoCProvider.RegisterType<CustomMessageBoxViewModel>();
            Mvx.IoCProvider.RegisterType<AddItemViewModel>();
            Mvx.IoCProvider.RegisterType<SplashScreenViewModel>();
            Mvx.IoCProvider.RegisterType<LoadListCollectionViewModel>();
            Mvx.IoCProvider.RegisterType<HwListCollectionViewModel>();
            Mvx.IoCProvider.RegisterType<SettingsViewModel>();
            Mvx.IoCProvider.RegisterType<HomePageViewModel>();

            RegisterAppStart<SplashScreenViewModel>();
        }
    }
}
