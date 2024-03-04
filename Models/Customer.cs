using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebApplicationYes.Models
{
    [DataContract]
    [XmlType("Customer")]
    public class Customer
    {
        [DataMember]
        public int EnovaID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}