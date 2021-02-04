using Cheque.API.Models;
using Cheque.API.Repositories.Interfaces;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cheque.API.Repositories.Implementation
{
    public class FakeChequeRepository : IChequeRepository
    {
        private ILogger<IChequeRepository> _logger;

        public FakeChequeRepository(ILogger<IChequeRepository> logger)
        {
            _logger = logger;
        }

        public List<ChequeDTO> GetChequesPack(int packSize)
        {
            _logger.LogInformation("Get {packSize} cheques", packSize);
            var result = new List<ChequeDTO>();
            for (var i = 0; i < packSize; i++)
            {
                result.Add(new ChequeDTO()
                {
                    Id = Guid.NewGuid(),
                    Number = (i + 1).ToString()
                });
            }
            _logger.LogInformation("Cheques : {result}", result);
            return result;
        }

        public int SaveCheque(ChequeDTO cheque)
        {
            _logger.LogInformation("New check : {cheque}", cheque);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), cheque.Id.ToString() + ".txt");
            File.WriteAllText(path, JsonConvert.SerializeObject(cheque));
            var reply = 1;
            _logger.LogInformation("Reply : {reply}", reply);
            return reply;
        }
    }
}