using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace WebApplicationYes.Models
{
    [DataContract]
    [XmlType("Document")]
    public class Document
    {
        [DataMember]
        public int EnovaID { get; set; }
        //[DataMember]
        //public string ContractorName { get; set; }
        //[DataMember]
        //public Address ContractorAddress { get; set; }
        //[DataMember]
        //public string ReceiverName { get; set; }
        //[DataMember]
        //public Address ReceiverAddress { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string ForeignNumber { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Date { get; set; }

      
        [DataMember]
        public Customer Customer { get; set; }
        [DataMember]
        [XmlElementAttribute("DocumentItems")]
        public List<DocumentItem> DocItems { get; set; }

        [DataMember]
        public decimal NetValue { get; set; }
        [DataMember]
        public decimal GrossValue { get; set; }
        [DataMember]
        public string Currency { get; set; }
    }
}