using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmailService;
using LoggerService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceExample
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ISLogger _fileLogger;
        private FileSystemWatcher watcher;
        private readonly string path = @"D:\galamshar\source\repos\WorkerServiceExample\TestFolder";
        private readonly string logPath = "D:\\galamshar\\source\\repos\\WorkerServiceExample\\log.txt";
        public Worker(ILogger<Worker> logger, IEmailSender emailSender, ISLogger fileLogger)
        {
            _logger = logger;
            _emailSender = emailSender;
            _fileLogger = fileLogger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            watcher = new FileSystemWatcher(path);

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            _fileLogger.ClearLog();
            _fileLogger.Log($"{DateTimeOffset.UtcNow} | Service started.");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _fileLogger.Log($"{DateTimeOffset.UtcNow} | Service stopped.");
            var message = new Message(new string[] { "etoneproject@gmail.com" }, $"Service Work Log | {DateTimeOffset.UtcNow.Date}", $"Service stopped at {DateTimeOffset.UtcNow}.", logPath);
            _emailSender.SendEmail(message).Wait();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                watcher.EnableRaisingEvents = true;

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var message = new Message(new string[] { "etoneproject@gmail.com" }, "Changed file in the filesystem", $"File {e.Name} {e.ChangeType}.", e.FullPath);
            _emailSender.SendEmail(message);
            _logger.LogInformation($"{DateTimeOffset.UtcNow} | File {e.Name} {e.ChangeType.ToString().ToLower()}.");
            _fileLogger.Log($"{DateTimeOffset.UtcNow} | File {e.Name} {e.ChangeType.ToString().ToLower()}.");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            var message = new Message(new string[] { "etoneproject@gmail.com" }, "Changed file in the filesystem", $"File {e.OldName} renamed to {e.Name}", null);
            _emailSender.SendEmail(message);
            _logger.LogInformation($"{DateTimeOffset.UtcNow} | File {e.OldName} renamed to {e.Name}");
            _fileLogger.Log($"{DateTimeOffset.UtcNow} | File {e.OldName} renamed to {e.Name}");
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var message = new Message(new string[] { "etoneproject@gmail.com" }, "Deleted file in the filesystem", $"File {e.Name} {e.ChangeType}.", null);
            _emailSender.SendEmail(message);
            _logger.LogInformation($"{DateTimeOffset.UtcNow} | File {e.Name} {e.ChangeType.ToString().ToLower()}.");
            _fileLogger.Log($"{DateTimeOffset.UtcNow} | File {e.Name} {e.ChangeType.ToString().ToLower()}.");
        }
    }
}
