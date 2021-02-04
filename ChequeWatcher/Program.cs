using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ChequeWatcher
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddLog4Net();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ChequeService(loggerFactory.CreateLogger<ChequeService>())
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
