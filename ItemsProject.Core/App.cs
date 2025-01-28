using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsProject.Core.Data;
using ItemsProject.Core.Databases;
using ItemsProject.Core.Messages;
using ItemsProject.Core.Services;
using ItemsProject.Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Presenters;
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

            Mvx.IoCProvider.RegisterType<ISqlDataAccess, SqlDataAccess>();
            Mvx.IoCProvider.RegisterType<IDatabaseData, SqlData>();
            Mvx.IoCProvider.RegisterType<IDataService, DataService>();
            Mvx.IoCProvider.RegisterSingleton(configuration);

            RegisterAppStart<BaseViewModel>();
        }

        
    }
}
