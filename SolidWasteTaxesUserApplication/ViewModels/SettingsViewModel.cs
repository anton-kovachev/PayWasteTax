using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWasteTaxesUserApplication.Model;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using SolidWasteTaxesUserApplication.Constants;
using SolidWasteTaxesUserApplication.Commands;

namespace SolidWasteTaxesUserApplication.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private SettingsModel _settingsModel = null;

        public SettingsViewModel()
        {
            _settingsModel = new SettingsModel();

            if (IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
            {
                _settingsModel.PersonalIdentityNumber = IsolatedStorageSettings.ApplicationSettings[Constants.ApplicationConstants.PersonalIdentityNumber].ToString();
                _settingsModel.PersonalFullName = IsolatedStorageSettings.ApplicationSettings[Constants.ApplicationConstants.PersonFullName].ToString();
            }
            else
            {
                _settingsModel.PersonalFullName = String.Empty;
                _settingsModel.PersonalIdentityNumber = String.Empty;
            }

        }

        public string  PersonFullName
        {
            get
            {
                return _settingsModel.PersonalFullName;
            }

            set
            {
                _settingsModel.PersonalFullName = value;
                NotifyPropertyChanged("PersonFullName");
            }
        }

        private string _personFullNameValidationLabelVisible = System.Windows.Visibility.Collapsed.ToString();
        public string PersonFullNameValidationLabelVisible
        {
            get
            {
                return _personFullNameValidationLabelVisible;
            }
        }

        public string PersonalIdentityNumber
        {
            get
            {
                return _settingsModel.PersonalIdentityNumber;
            }

            set
            {
                _settingsModel.PersonalIdentityNumber = value;
                NotifyPropertyChanged("PersonalIdentityNumber");
            }
        }

        private string _personalIdentityValidationLabelVisible = System.Windows.Visibility.Collapsed.ToString();
        public string PersonalIdentityValidationLabelVisible
        {
            get
            {
                return _personalIdentityValidationLabelVisible;
            }
        }

        private string _settingsDataVisible = System.Windows.Visibility.Visible.ToString();
        public string SettingsDataVisible
        {
            get
            {
                return _settingsDataVisible;
            }
        }

        private string _saveMessageVisible = System.Windows.Visibility.Collapsed.ToString();
        public string SaveMessageVisible
        {
            get
            {
                return _saveMessageVisible;
            }
        }

        private ICommand _savePersonalInformationInSettingsCommand;
        public ICommand SavePersonalInformationInSettingsCommand
        {
            get
            {
               if(_savePersonalInformationInSettingsCommand == null)
               {
                   _savePersonalInformationInSettingsCommand = new RelayCommand(param => SavePersonalInformationInSettings(), param => CanSavePersonalInformation());
               }

               return _savePersonalInformationInSettingsCommand;
            }
        }

        private void SavePersonalInformationInSettings()
        {
            if(PersonFullName == String.Empty)
            {
                return;
            }

            if(PersonalIdentityNumber == String.Empty)
            {
                return;
            }


            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            appSettings[ApplicationConstants.PersonFullName] = PersonFullName;
            appSettings[ApplicationConstants.PersonalIdentityNumber] = PersonalIdentityNumber;

            appSettings.Save();

            _settingsDataVisible = System.Windows.Visibility.Collapsed.ToString();
            NotifyPropertyChanged("SettingsDataVisible");

            _saveMessageVisible = System.Windows.Visibility.Visible.ToString();
            NotifyPropertyChanged("SaveMessageVisible");
        }

        private int count = 0;
        private bool CanSavePersonalInformation()
        {
            count++;

            bool canSaveSettings = true;

            if(count == 1)
            {
                return canSaveSettings;
            }

            if (PersonFullName == null || PersonFullName == String.Empty)
            {
                _personFullNameValidationLabelVisible = System.Windows.Visibility.Visible.ToString();
                NotifyPropertyChanged("PersonFullNameValidationLabelVisible");

                canSaveSettings = false;
            }
            else
            {
                _personFullNameValidationLabelVisible = System.Windows.Visibility.Collapsed.ToString();
                NotifyPropertyChanged("PersonFullNameValidationLabelVisible");


            }

            if (PersonalIdentityNumber == null || PersonalIdentityNumber == String.Empty)
            {
                _personalIdentityValidationLabelVisible = System.Windows.Visibility.Visible.ToString();
                NotifyPropertyChanged("PersonalIdentityValidationLabelVisible");

                canSaveSettings = false;
            }
            else
            {
                _personalIdentityValidationLabelVisible = System.Windows.Visibility.Collapsed.ToString();
                NotifyPropertyChanged("PersonalIdentityValidationLabelVisible");
            }

            return canSaveSettings;
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
