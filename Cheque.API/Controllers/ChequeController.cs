using Cheque.API.Models;
using Cheque.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Cheque.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChequeController : ControllerBase
    {
        private readonly ILogger<IChequeRepository> _logger;
        private readonly IChequeRepository _chequeRepository;

        public ChequeController(ILogger<IChequeRepository> logger, IChequeRepository chequeRepository)
        {
            _logger = logger;
            _chequeRepository = chequeRepository;
        }

        [HttpGet]
        public List<ChequeDTO> GetCheques(int count)
        {
            _logger.LogInformation("Get cheques");
            return _chequeRepository.GetChequesPack(count);
        }
        [HttpPost]
        public int SendCheque(ChequeDTO cheque)
        {
            _logger.LogInformation("Send cheque");
            return _chequeRepository.SaveCheque(cheque);
        }
    }
}
