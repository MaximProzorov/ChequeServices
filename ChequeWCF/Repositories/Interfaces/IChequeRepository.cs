using ChequeWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChequeWCF.Repositories.Interfaces
{
    public interface IChequeRepository
    {
        int SaveCheque(Cheque cheque);
        List<Cheque> GetChequesPack(int packSize);
    }
}
