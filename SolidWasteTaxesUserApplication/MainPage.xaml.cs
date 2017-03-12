using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SolidWasteTaxesUserApplication.Resources;
using SolidWasteTaxesUserApplication.ViewModels;
using System.Windows.Data;
using System.Globalization;
using System.IO.IsolatedStorage;
using SolidWasteTaxesUserApplication.Constants;

namespace SolidWasteTaxesUserApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SetDemoUserSettings();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            var specificWasteImageSelected = (MainLongListSelector.SelectedItem as ImageViewModel);

            MainLongListSelector.Visibility = System.Windows.Visibility.Collapsed;

            SpecificWasteRequestSentMessageTextBlock.Text = specificWasteImageSelected.RequestSentMessage;
            SpecificWasteRequestSentMessageTextBlock.Visibility = System.Windows.Visibility.Visible;
        }

        private void SetDemoUserSettings()
        {
            if(IsolatedStorageSettings.ApplicationSettings.Contains(Constants.ApplicationConstants.PersonalIdentityNumber))
            {
                return;
            }

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            appSettings[ApplicationConstants.PersonFullName] = ApplicationConstants.DemoUserFullName;
            appSettings[ApplicationConstants.PersonalIdentityNumber] = ApplicationConstants.DemoUserIdentityNumber;

            appSettings.Save();
        }

        private void TaxesInformationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationaPanel.Visibility = System.Windows.Visibility.Collapsed;
            TaxesInformationContent.Visibility = System.Windows.Visibility.Visible;
            SetFooter();

            ObligationViewModel obligationViewModel = new ObligationViewModel();

            obligationViewModel.CurrentYearObligations = new Decimal(200);
            obligationViewModel.PreviousYearsObligations = new Decimal(150);
            obligationViewModel.InterestsAmount = new Decimal(50);
            obligationViewModel.SpecificWasteObligations = new Decimal(70);

            DataContext = obligationViewModel;
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            FillListBoxes();
            TaxesInformationContent.Visibility = System.Windows.Visibility.Collapsed;
            PaymentInformationContent.Visibility = System.Windows.Visibility.Visible;

            DataContext = new PaymentViewModel();
        }

        private void FillListBoxes()
        {
            FillCreditCardTypeList();
            FillMonthList();
            FillYearLlist();
        }

        private void FillCreditCardTypeList()
        {
            CreditCardTypeTextBox.Items.Clear();

            CreditCardTypeTextBox.Items.Add("Visa");
            CreditCardTypeTextBox.Items.Add("MasterCard");
            CreditCardTypeTextBox.Items.Add("Eurocard");
            CreditCardTypeTextBox.Items.Add("American Express");
        }

        private void FillMonthList()
        {
            MonthList.Items.Clear();

            int firstMonth = 1;
            int endMonth = 12;

            for (int currentMonth = firstMonth; currentMonth <= endMonth; ++currentMonth)
            {
                MonthList.Items.Add(currentMonth);
            }
        }

        private void FillYearLlist()
        {
            YearList.Items.Clear();

            long thisYear = DateTime.Now.Year;
            long lastPossibleYear = thisYear + 10;

            for (long currentYear = thisYear; currentYear <= lastPossibleYear; currentYear++)
            {
                YearList.Items.Add(currentYear);
            }
        }

        private void SpecificWasteRequestButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationaPanel.Visibility = System.Windows.Visibility.Collapsed;
            SetFooter();
            SpecificWasteRequestContent.Visibility = System.Windows.Visibility.Visible;
            MainLongListSelector.Visibility = System.Windows.Visibility.Visible;
            SpecificWasteRequestSentMessageTextBlock.Visibility = System.Windows.Visibility.Collapsed;

            DataContext = new SpecificWasteViewModel();
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationaPanel.Visibility = System.Windows.Visibility.Collapsed;
            SettingsContent.Visibility = System.Windows.Visibility.Visible;
            SetFooter();

            DataContext = new SettingsViewModel();
        }

        private void SetFooter()
        {
            imgHome.Margin = new Thickness(154, 10, 252, 6);
            imgHome.Visibility = System.Windows.Visibility.Visible;
            imgHome.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            imgAbout.Margin = new Thickness(247, 10, 155, 6);
            imgAbout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        }

        private void Home_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GoToHomePage();
        }

        private void About_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void GoToHomePage()
        {
            TaxesInformationContent.Visibility = System.Windows.Visibility.Collapsed;
            PaymentInformationContent.Visibility = System.Windows.Visibility.Collapsed;
            SpecificWasteRequestContent.Visibility = System.Windows.Visibility.Collapsed;
            SettingsContent.Visibility = System.Windows.Visibility.Collapsed;

            imgHome.Visibility = System.Windows.Visibility.Collapsed;
            imgAbout.Margin = new Thickness(147, 10, 155, 6);
            imgAbout.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            
            NavigationaPanel.Visibility = System.Windows.Visibility.Visible;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}