using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWasteTaxesUserApplication.Model
{
    public class SpecificWasteModel
    {
        public string SenderPersonalIdentityNumber { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Measurement { get; set; }
        public string Settlement { get; set; }
        public DateTime TransportationRequestDate { get; set; }
    }
}
