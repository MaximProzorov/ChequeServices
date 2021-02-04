using ChequeWCF.Models;
using ChequeWCF.Repositories.Implementation;
using ChequeWCF.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChequeWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class ChequeService : IChequeService
    {
        IChequeRepository _chequeRepository;
        public ChequeService()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var isFakeImplementation = !bool.TryParse(ConfigurationManager.AppSettings["IsFakeImplementation"], out var result) || result;
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddLog4Net();
            if (!isFakeImplementation)
                _chequeRepository = new ChequeRepository(new SqlConnection(connectionString), loggerFactory.CreateLogger<IChequeRepository>());
            else
                _chequeRepository = new FakeChequeRepository(loggerFactory.CreateLogger<IChequeRepository>());
        }

        public List<Cheque> GetCheques(int count)
        {
            return _chequeRepository.GetChequesPack(count);
        }

        public int SendCheque(Cheque cheque)
        {
            return _chequeRepository.SaveCheque(cheque);
        }
    }
}
