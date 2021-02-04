using ChequeWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChequeWCF
{
    [ServiceContract]
    public interface IChequeService
    {
        [OperationContract]
        int SendCheque(Cheque cheque);

        [OperationContract]
        List<Cheque> GetCheques(int count);
    }
}
