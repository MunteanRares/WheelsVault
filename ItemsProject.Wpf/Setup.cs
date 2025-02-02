using ItemsProject.Core.Databases;
using ItemsProject.Core.Messages;
using ItemsProject.Core.ViewModels;
using ItemsProject.Wpf.MessageBoxes;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Serilog;
using Serilog.Extensions.Logging;

namespace ItemsProject.WPF
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override void InitializeApp(IMvxApplication app)
        {
            base.InitializeApp(app);
            Mvx.IoCProvider.RegisterType<IMessageBoxService, MessageBoxService>();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}