using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebApplicationYes.Models
{
    [DataContract]
    [XmlType("Address")]
    public class Address
    {
        [DataMember]
        public int EnovaID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal NetPrice { get; set; }
        [DataMember]
        public decimal GrossPrice { get; set; }
        [DataMember]
        public decimal DiscountPercent { get; set; }
    }
}