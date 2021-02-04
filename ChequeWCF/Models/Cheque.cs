using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChequeWCF.Models
{
    [DataContract]
    public class Cheque
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public decimal Summ { get; set; }
        [DataMember]
        public decimal Discount { get; set; }
        [DataMember]
        public string[] Articles { get; set; }
    }
}