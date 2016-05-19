using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"CSVFile.csv");

            XElement xml = new XElement("Root",
                from str in lines
                let columns = str.Split(',')
                select new XElement("Employee",
                    new XElement("ID", columns[0]),
                    new XElement("Name", columns[1]),
                    new XElement("Title", columns[2]),
                    new XElement("City", columns[3]),
                    new XElement("Region", columns[4]),
                    new XElement("Country", columns[5])
                )
            );
            xml.Save(@"XMLFile.xml");
        }
    }
}
