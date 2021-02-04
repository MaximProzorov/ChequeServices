using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Logging;
using Cheque.API.Models;
using Cheque.API.Repositories.Interfaces;

namespace Cheque.API.Repositories.Implementation
{
    public class ChequeRepository : IChequeRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<IChequeRepository> _logger;
        public ChequeRepository(IDbConnection dbConnection, ILogger<IChequeRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }
        public List<ChequeDTO> GetChequesPack(int packSize)
        {
            _logger.LogInformation("Get {packSize} cheques", packSize);
            var reply = _dbConnection.Query<ChequeDTO>("get_cheques_pack", packSize, commandType: CommandType.StoredProcedure).ToList();
            _logger.LogInformation("Cheques : {reply}", reply);
            return reply;
        }

        public int SaveCheque(ChequeDTO cheque)
        {
            _logger.LogInformation("New check : {cheque}", cheque);
            var reply = _dbConnection.Execute("save_cheques", cheque, commandType: CommandType.StoredProcedure);
            _logger.LogInformation("Reply : {reply}", reply);
            return reply;
        }
    }
}