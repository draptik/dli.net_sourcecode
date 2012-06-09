using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ploeh.Samples.CommerceService
{
    [DataContract(Namespace = "urn:ploeh:productMgtSrvc")]
    public class ProductContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public MoneyContract UnitPrice { get; set; }
    }
}
