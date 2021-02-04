using Cheque.API.Models;
using System.Collections.Generic;

namespace Cheque.API.Repositories.Interfaces
{
    public interface IChequeRepository
    {
        int SaveCheque(ChequeDTO cheque);
        List<ChequeDTO> GetChequesPack(int packSize);
    }
}
