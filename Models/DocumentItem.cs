using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebApplicationYes.Models
{
    [DataContract]
    [XmlType("DocumentItem")]
    public class DocumentItem
    {
        [DataMember]
        public int EnovaID { get; set; }
        [DataMember]
        public int ProductEnovaID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public double Qty { get; set; }
        [DataMember]
        public double? RemainingQty { get; set; }
        [DataMember]
        public string Unit { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string PriceType { get; set; }

        [DataMember]
        public double Tax { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}