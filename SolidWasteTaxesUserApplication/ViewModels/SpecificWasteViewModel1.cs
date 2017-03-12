using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWasteTaxesUserApplication.Model;

namespace SolidWasteTaxesUserApplication.ViewModels
{
    class SpecificWasteViewModel1 : INotifyPropertyChanged
    {
        private SpecificWasteModel _specificWasteModel;

        public SpecificWasteViewModel1()
        {
            _specificWasteModel = new SpecificWasteModel();
        }


        public string SenderPersonalIdentityNumber
        {
            get
            {
                return _specificWasteModel.SenderPersonalIdentityNumber;
            }

            set
            {
                _specificWasteModel.SenderPersonalIdentityNumber = value;
                NotifyPropertyChanged("SenderPersonalIdentityNumber");
            }
        }

        public string Type
        {
            get
            {
                return _specificWasteModel.Type;
            }

            set
            {
                _specificWasteModel.Type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public int Amount
        {
            get
            {
                return _specificWasteModel.Amount;
            }

            set
            {
                _specificWasteModel.Amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        public string Measurement
        {
            get
            {
                return _specificWasteModel.Measurement;
            }

            set
            {
                _specificWasteModel.Measurement = value;
                NotifyPropertyChanged("Measurement");

            }
        }

        public string Settlement
        {
            get
            {
                return _specificWasteModel.Settlement;
            }

            set
            {
                _specificWasteModel.Settlement = value;
                NotifyPropertyChanged("Settlement");
            }
        }

        public DateTime TransportationRequestDate
        {
            get
            {
                return _specificWasteModel.TransportationRequestDate;
            }

            set
            {
                _specificWasteModel.TransportationRequestDate = value;
                NotifyPropertyChanged("TransportationRequestDate");
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
}
