using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleUI
{
    public class operation
    {
        [XmlAttribute(AttributeName ="type")]
        public string OperationType { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
