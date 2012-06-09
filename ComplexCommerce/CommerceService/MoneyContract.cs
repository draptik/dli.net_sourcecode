using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ploeh.Samples.CommerceService
{
    [DataContract(Namespace = "urn:ploeh:productMgtSrvc")]
    public class MoneyContract
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }
    }
}
