using ChequeWatcher.ChequeServiceWCF;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;

namespace ChequeWatcher
{
    public partial class ChequeService : ServiceBase
    {
        private string _garbagePath;
        private string _completePath;
        private IChequeService _chequeService;
        private readonly ILogger _logger;
        public ChequeService(ILogger logger)
        {
            _logger = logger;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            _garbagePath = ConfigurationManager.AppSettings["GarbagePath"];
            _completePath = ConfigurationManager.AppSettings["CompletePath"];
            var folderPath = ConfigurationManager.AppSettings["FolderPath"];

            _chequeService = new ChequeServiceClient();

            var fileWatcher = new FileSystemWatcher
            {
                Path = folderPath,
                IncludeSubdirectories = false,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                | NotifyFilters.Attributes | NotifyFilters.DirectoryName | NotifyFilters.FileName
                                | NotifyFilters.Security | NotifyFilters.Size
            };
            fileWatcher.Created += OnChanged;
            fileWatcher.EnableRaisingEvents = true;

            _logger.LogInformation("Start watch in {folderPath}, garbage path : {_garbagePath}, complete path : {_completePath} ", folderPath, _garbagePath, _completePath);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            if (File.GetAttributes(filePath) != FileAttributes.Directory)
            {
                if (Path.GetExtension(filePath) == ".txt")
                {
                    Cheque cheque = null;
                    try
                    {
                        var json = File.ReadAllText(filePath);
                        _logger.LogInformation("File text : {json}", json);
                        cheque = JsonConvert.DeserializeObject<Cheque>(json);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError("JSON deserialize error : {ex}", ex);
                    }
                    try
                    {
                        _logger.LogInformation("try send cheque");
                        if (cheque != null && _chequeService.SendCheque(cheque) > 0)
                        {
                            MoveWithUniqueName(filePath, Path.Combine(_completePath, Path.GetFileName(filePath)));
                            _logger.LogInformation("file moved in Complete");
                            return;
                        }
                    }
                    catch(EndpointNotFoundException ex)
                    {
                        _logger.LogError("Error : {ex}", ex);
                        return;
                    }
                }
                MoveWithUniqueName(filePath, Path.Combine(_garbagePath, Path.GetFileName(filePath)));
                _logger.LogInformation("file moved in Garbge");
            }
        }

        private void MoveWithUniqueName(string sourcePath, string destinationPath)
        {
            int count = 1;
            var fileName = Path.GetFileNameWithoutExtension(sourcePath);
            var extension = Path.GetExtension(sourcePath);
            var path = Path.GetDirectoryName(destinationPath);
            var filePath = destinationPath;
            while (File.Exists(filePath))
            {
                string tempFileName = string.Format("{0}({1})", fileName, count++);
                filePath = Path.Combine(path, tempFileName + extension);
            }
            File.Move(sourcePath, filePath);
        }
        protected override void OnStop()
        {
        }
    }
}
