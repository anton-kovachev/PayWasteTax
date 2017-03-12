using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWasteTaxesUserApplication.Model;
using System.Collections.ObjectModel;
using SolidWasteTaxesUserApplication.Resources;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using SolidWasteTaxesUserApplication.Commands;


namespace SolidWasteTaxesUserApplication.ViewModels
{
    class ImageViewModel : INotifyPropertyChanged
    {

         public ImageViewModel()
        {
        }

        public ImageViewModel(string sourceURI)
        {
            imageURI = sourceURI;
        }

        private string imageURI;
        public string ImageURI
        {
            get
            {
                return imageURI;
            }

            set
            {
                imageURI = value;
                NotifyPropertyChanged("ImageViewModel");
            }
        }

        private string _label = string.Empty;
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = value;
                NotifyPropertyChanged("Text");
            }
        }

        public string RequestSentMessage
        {
            get
            {
                return Label + AppResources.SpecificWasteRequestSentLabel;
            }
        }

        private string _requestLabelVisible =  System.Windows.Visibility.Collapsed.ToString();
        public string RequestSentInfoVisible
        {
            get
            {
                return _requestLabelVisible;
            }

            set
            {
                _requestLabelVisible = value;
                NotifyPropertyChanged("RequestSentLabelVisible");
            }
        }

        private string _specificWasteButtonsVisible = System.Windows.Visibility.Visible.ToString();
        public string SpecificWasteButtonsVisible
        {
            get
            {
                return _specificWasteButtonsVisible;
            }

            set
            {
                _specificWasteButtonsVisible = value;
                NotifyPropertyChanged("SpecificWasteButtonsVisible");
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

        private ICommand _submitSpecificRequestCommand = null;
        
        public ICommand SubmitSpecificRequestCommand
        {
            get
            {
                if(_submitSpecificRequestCommand == null)
                {
                    _submitSpecificRequestCommand = new RelayCommand(param => SendSpecificWasteRequest(), (param) => { return true; });
                }

                return _submitSpecificRequestCommand;
            }
        }

        private void SendSpecificWasteRequest()
        {
            RequestSentInfoVisible = System.Windows.Visibility.Visible.ToString();
            SpecificWasteButtonsVisible = System.Windows.Visibility.Collapsed.ToString();

        //    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));

        //    RequestSentInfoVisible = System.Windows.Visibility.Collapsed.ToString();
        //    SpecificWasteButtonVisible = System.Windows.Visibility.Visible.ToString();
        }

        public static IEnumerable<ImageViewModel> ImageViewModelFactory()
        {
            List<ImageViewModel> imageViewModelList = new List<ImageViewModel>();
            
            int imageCount = 6;
            string URI = "images/img_btn_q";
            string suffix = "_New.png";

            for(int i = 1; i <= imageCount; ++i)
            {
                string fullURI = URI + i.ToString() + suffix;
                ImageViewModel imageViewModel = new ImageViewModel(fullURI);
                imageViewModelList.Add(imageViewModel);
            }

            imageViewModelList[0].Label = AppResources.AccumulatorLabel;
            imageViewModelList[0].ImageURI = "images/img_btn_q1_New.png";
            imageViewModelList[1].Label = AppResources.ConstructionLabel;
            imageViewModelList[1].ImageURI = "images/img_btn_q2_New.png";
            imageViewModelList[2].Label = AppResources.ElectronicsLabel;
            imageViewModelList[3].Label = AppResources.MedicalWasteLabel;
            imageViewModelList[4].Label = AppResources.ChemicalWasteLabel;
            imageViewModelList[5].Label = AppResources.AnimalWasteLabel;
 
            return imageViewModelList;
        }
    }

    class SpecificWasteViewModel : INotifyPropertyChanged
    {
        public SpecificWasteViewModel()
        {
            SpecificWasteImages = new ObservableCollection<ImageViewModel>();
            foreach(ImageViewModel imageViewModel in ImageViewModel.ImageViewModelFactory())
            {
                SpecificWasteImages.Add(imageViewModel);
            }
        }

        public string SpecificWasteRequestVisible
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
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

        public ObservableCollection<ImageViewModel> SpecificWasteImages { get; private set; }

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
