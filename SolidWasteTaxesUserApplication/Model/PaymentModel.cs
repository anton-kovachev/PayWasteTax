using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWasteTaxesUserApplication.Model
{
    class PaymentModel
    {
        public decimal TotalPaymentAmount { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardExpirationDate { get; set; }
        public string CreditCardHolderName { get; set; }
    }
}
