using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("cars")]
    class ExportCarsWithNameBMWDto
    {
        public int CarId { get; set; }

        public mo Type { get; set; }
    }
}
