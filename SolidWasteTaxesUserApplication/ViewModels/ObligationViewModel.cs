using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWasteTaxesUserApplication.Model;
using System.Windows.Input;
using System.Net;
using Newtonsoft.Json;
using SolidWasteTaxesUserApplication.Commands;
using SolidWasteTaxesUserApplication.Resources;
using System.IO.IsolatedStorage;

namespace SolidWasteTaxesUserApplication.ViewModels
{
    class ObligationViewModel : INotifyPropertyChanged
    {
        private ObligationModel _obligationModel;
 
        public ObligationViewModel()
        {
            _obligationModel = new ObligationModel();
            GetTaxes();
        }

        public string TaxesInformationVisible
        {
            get
            {
                if(IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
                {
                    return System.Windows.Visibility.Visible.ToString();
                }
                else
                {
                    return System.Windows.Visibility.Collapsed.ToString();
                }
            }
        }

        public string MissingPersonalDataMessageVisible
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
                {
                    return System.Windows.Visibility.Collapsed.ToString();
                }
                else
                {
                    return System.Windows.Visibility.Visible.ToString();
                }
            }
        }

        public string DebtorIdentityNumber
        {
            get
            {
                if(_obligationModel.DebtorIdentityNumber == null || _obligationModel.DebtorIdentityNumber == String.Empty)
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
                    {
                        _obligationModel.DebtorIdentityNumber = IsolatedStorageSettings.ApplicationSettings[Constants.ApplicationConstants.PersonalIdentityNumber].ToString();
                    }
                    else
                    {
                        _obligationModel.DebtorIdentityNumber = String.Empty;
                    }
                }

                return  _obligationModel.DebtorIdentityNumber;
            }
        }

        public decimal CurrentYearObligations
        {
            get
            {
                return _obligationModel.CurrentYearObligations;
            }

            set
            {
                _obligationModel.CurrentYearObligations = value;
                NotifyPropertyChanged("CurrentYearObligations");
            }
        }

        public decimal PreviousYearsObligations
        {
            get
            {
                return _obligationModel.PreviousYearsObligations;
            }

            set
            {
                _obligationModel.PreviousYearsObligations = value;
                NotifyPropertyChanged("PreviousYearsObligations");
            }
        }

        public decimal InterestsAmount
        {
            get
            {
                return _obligationModel.InterestsAmount;
            }

            set
            {
                _obligationModel.InterestsAmount = value;
                NotifyPropertyChanged("InterestsAmount");
            }
        }

        public decimal SpecificWasteObligations
        {
            get
            {
                return _obligationModel.SpecificWasteObligations;
            }

            set
            {
                _obligationModel.SpecificWasteObligations = value;
                NotifyPropertyChanged("SpecificWasteObligations");
            }
        }

        public decimal TotalObligations
        {
            get
            {
                return CurrentYearObligations + PreviousYearsObligations + InterestsAmount + SpecificWasteObligations; 
            }
        }

        private bool _dataLoaded = false;
        public bool DataLoaded
        {
            get
            {
                return _dataLoaded;
            }

            set
            {
                _dataLoaded = value;
                NotifyPropertyChanged("DataLoaded");
                NotifyPropertyChanged("InformationVisible");
                NotifyPropertyChanged("PayButtonVisible");
    
            }
        }

        public string InformationVisible
        {
            get
            {
                if(DataLoaded == true)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }

        private string _waitingMessageVisible = System.Windows.Visibility.Collapsed.ToString();
        public string WaitingMessageVisible
        {
            get
            {
                return _waitingMessageVisible;
            }

            set
            {
                _waitingMessageVisible = value;
                NotifyPropertyChanged("WaitingMessageVisible");
            }
        }

        private string _downloadPercent = "0";
        public string DownloadPercent
        {
            get
            {
                return String.Format(AppResources.WaitingLabel, _downloadPercent);
            }

            set
            {
                _downloadPercent = value;
                NotifyPropertyChanged("DownloadPercent");
            }
        }

        private string _failureInformationVisible =  System.Windows.Visibility.Collapsed.ToString();
        public string FailureInformationMessageVisible
        {
            get
            {
                return _failureInformationVisible;                
            }

            set
            {
                _failureInformationVisible = value;
                NotifyPropertyChanged("FailureInformationLabelVisible");
            }
        }

        public string PayButtonVisible
        {
            get
            {
                if(DataLoaded)
                {
                    return System.Windows.Visibility.Visible.ToString();
                }
                else
                {
                    return System.Windows.Visibility.Collapsed.ToString();
                }
            }
        }


        private ICommand _getTaxesCommand;
        public ICommand GetTaxesCommand
        {
            get
            {
                if (_getTaxesCommand == null)
                {
                    _getTaxesCommand = new RelayCommand(param => GetTaxes(), param => CanGetTaxes());
                }

                return _getTaxesCommand;
            }

        }

        private void GetTaxes()
        {
            LoadData();
        }

        private bool CanGetTaxes()
        {
            if(DebtorIdentityNumber == null)
            {
                return false;
            }
            
            if (DebtorIdentityNumber == String.Empty)
            {
                return false;
            }

            return true;
        }

        private const string apiUrl = @"http://localhost:5555/api/obligation";

        public void LoadData()
        {
            LoadDataStart();

            WebClient webClient = new WebClient();
            webClient.Headers["Accept"] = "application/json";
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadCatalogCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Progress);
            string realUrl = apiUrl + @"/" + DebtorIdentityNumber;
            webClient.DownloadStringAsync(new Uri(realUrl));
        }

        private void LoadDataStart()
        {
            DataLoaded = false;
            WaitingMessageVisible = System.Windows.Visibility.Visible.ToString();
            FailureInformationMessageVisible = System.Windows.Visibility.Collapsed.ToString();
        }

        private void Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadPercent = e.ProgressPercentage.ToString();
        }
        private void webClient_DownloadCatalogCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                WaitingMessageVisible = System.Windows.Visibility.Collapsed.ToString();
                if (e.Result != null)
                {
                    var obligation = JsonConvert.DeserializeObject<ObligationModel>(e.Result);

                    CurrentYearObligations = obligation.CurrentYearObligations;
                    PreviousYearsObligations = obligation.PreviousYearsObligations;
                    InterestsAmount = obligation.InterestsAmount;
                    SpecificWasteObligations = obligation.SpecificWasteObligations;

                    LoadDataEnd();
                }
            }
            catch (Exception ex)
            {
                FailureInformationMessageVisible = System.Windows.Visibility.Visible.ToString();
            }
        }

        private void LoadDataEnd()
        {
            DataLoaded = true;
            DownloadPercent = "0";
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
