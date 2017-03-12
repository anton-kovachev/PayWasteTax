using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWasteTaxesUserApplication.Model;
using System.Net;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using SolidWasteTaxesUserApplication.Commands;

namespace SolidWasteTaxesUserApplication.ViewModels
{
    class CreditCardTypeViewModel : INotifyPropertyChanged
    {
        private string _creditCardType = null;
        
        public string CreditCardType
        {
            get
            {
                return _creditCardType;
            }

            set
            {
                _creditCardType = value;
                NotifyPropertyChanged("CreditCardType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    class PaymentViewModel : INotifyPropertyChanged
    {
        private PaymentModel _paymentModel;
        public PaymentViewModel()
        {
            _paymentModel = new PaymentModel();
            //CreditCardTypes = new ObservableCollection<CreditCardTypeViewModel>();

            //CreditCardTypes.Add(new CreditCardTypeViewModel(){ CreditCardType = "efewfwefwe"});
            //CreditCardTypes.Add(new CreditCardTypeViewModel() { CreditCardType = "cdvsdv" });
            //LoadCreditCardTypes();
            LoadObligations();
        }

        public decimal TotalPaymentAmount
        {
            get
            {
                return _paymentModel.TotalPaymentAmount;
            }
        }

        public string CreditCardType
        {
            get
            {
                return _paymentModel.CreditCardType;
            }

            set
            {
                _paymentModel.CreditCardType = value;
                NotifyPropertyChanged("CreditCardType");
            }
        }

        public string CreditCardNumber
        {
            get
            {
                return _paymentModel.CreditCardNumber;
            }


            set
            {
                _paymentModel.CreditCardNumber = value;
                NotifyPropertyChanged("CreditCardNumber");
            }
        }

        private string _creditCardNumberValidationMessageVisible = System.Windows.Visibility.Collapsed.ToString();
        public string CreditCardNumberValidationMessageVivisble
        {
            get
            {
                return _creditCardNumberValidationMessageVisible;
            }
        }

        public string CreditCardHolderName
        {
            get
            {
                return _paymentModel.CreditCardHolderName;
            }

            set
            {
                _paymentModel.CreditCardHolderName = value;
                NotifyPropertyChanged("CreditCardHolderName");
            }
        }

        private string _creditCardHolderNameValidationMessageVisible = System.Windows.Visibility.Collapsed.ToString();
        public string CreditCardHolderNameValidationMessageVisisble
        {
            get
            {
                return _creditCardHolderNameValidationMessageVisible;
            }
        }

        private bool _paymentMade = false;

        public string PaymentMadeLabelVisible
        {
            get
            {
                  if(_paymentMade == true)
                  {
                      return System.Windows.Visibility.Visible.ToString();
                  }

                  return System.Windows.Visibility.Collapsed.ToString();
            }
          
        }

        public string PaymentFormVisible
        {
            get
            {
                if(_paymentMade == false)
                {
                    return System.Windows.Visibility.Visible.ToString();
                }

                return System.Windows.Visibility.Collapsed.ToString();
            }
        }

        private ICommand _paymentCommand = null;
        public ICommand PaymentCommand
        {
            get
            {
                if(_paymentCommand == null)
                {
                    _paymentCommand = new RelayCommand(param => ExecutePayment(), param => CanExecutePayment());
                }

                return _paymentCommand;
            }
        }
 

        private void ExecutePayment()
        {
            _paymentMade = true;
            NotifyPropertyChanged("PaymentMadeLabelVisible");
            NotifyPropertyChanged("PaymentFormVisible");
        }

        private int count = 0;
        private bool CanExecutePayment()
        {
            count++;

            bool canProcessPayment = true;

            if(count == 1)
            {
                return canProcessPayment;
            }

            if(CreditCardNumber == null || CreditCardNumber == String.Empty)
            {
                canProcessPayment = false;

                _creditCardNumberValidationMessageVisible = System.Windows.Visibility.Visible.ToString();
                NotifyPropertyChanged("CreditCardNumberValidationMessageVivisble");

            }
            else
            {
                _creditCardNumberValidationMessageVisible = System.Windows.Visibility.Collapsed.ToString();
                NotifyPropertyChanged("CreditCardNumberValidationMessageVivisble");
            }

            if(CreditCardHolderName == null || CreditCardHolderName == String.Empty)
            {
                canProcessPayment = false;

                _creditCardHolderNameValidationMessageVisible = System.Windows.Visibility.Visible.ToString();
                NotifyPropertyChanged("CreditCardHolderNameValidationMessageVisisble");
            }
            else
            {
                _creditCardHolderNameValidationMessageVisible = System.Windows.Visibility.Collapsed.ToString();
                NotifyPropertyChanged("CreditCardHolderNameValidationMessageVisisble");
            }

            return canProcessPayment;
        }

        //public ObservableCollection<CreditCardTypeViewModel> CreditCardTypes { get; private set; }

        private const string obligationUrl = @"http://localhost:5555/api/obligation";

        private void LoadObligations()
        {
            WebClient webClient = new WebClient();
            webClient.Headers["Accept"] = "application/json";
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadObligationCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Progress);

            string personalIdentityNumber = IsolatedStorageSettings.ApplicationSettings[Constants.ApplicationConstants.PersonalIdentityNumber].ToString();
            string apiUrl = obligationUrl + @"/" + personalIdentityNumber;

            webClient.DownloadStringAsync(new Uri(apiUrl));
        }

        private void webClient_DownloadObligationCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    var obligation = JsonConvert.DeserializeObject<ObligationModel>(e.Result);

                    _paymentModel.TotalPaymentAmount = obligation.CurrentYearObligations + obligation.PreviousYearsObligations + obligation.SpecificWasteObligations + obligation.InterestsAmount;
                    NotifyPropertyChanged("TotalPaymentAmount");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Progress(object sender, DownloadProgressChangedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
