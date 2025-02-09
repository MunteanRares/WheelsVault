using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Serilog;
using Serilog.Extensions.Logging;

namespace ItemsProject.WPF
{
    public class Setup : MvxWpfSetup<Core.App>
    {
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