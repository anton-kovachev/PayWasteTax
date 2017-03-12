using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWasteTaxesUserApplication.Model
{
    public class ObligationModel
    {
        public string DebtorIdentityNumber { get; set; }
        public decimal CurrentYearObligations { get; set; }
        public decimal PreviousYearsObligations { get; set; }
        public decimal InterestsAmount { get; set; }
        public decimal SpecificWasteObligations { get; set; }
    }
}
