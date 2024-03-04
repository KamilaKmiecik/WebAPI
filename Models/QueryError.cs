using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WebApplicationYes.Models
{ 
    [DataContract]
    [XmlType("QueryError")]
    public class QueryError
    {
        [DataMember]
        public string ErrorText { get; set; }

        public static QueryError Parse(Exception ex) => new QueryError() { ErrorText = ex.Message };
    }
}