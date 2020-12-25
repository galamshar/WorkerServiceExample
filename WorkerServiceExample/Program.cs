using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using LoggerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerServiceExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    var emailConfig = hostContext.Configuration.GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>();
                    services.AddSingleton(emailConfig);

                    var fileLoggerConfig = hostContext.Configuration.GetSection("FileLoggerConfiguration")
                    .Get<FileLoggerConfiguration>();
                    services.AddSingleton(fileLoggerConfig);

                    services.AddSingleton<IEmailSender, EmailSender>();
                    services.AddSingleton<ISLogger, FileLogger>();
                    services.AddHostedService<Worker>();
                });
    }
}
