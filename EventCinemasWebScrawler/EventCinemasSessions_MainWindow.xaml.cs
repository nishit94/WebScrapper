using EventCinemasWebScrawler.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EventCinemasWebScrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedLocation;
        private string selectedDate;
        private DateTime SelectedDate = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();

            /// Setting up DatePicker Control
            MovieDate.DisplayDateStart = DateTime.Today;
            MovieDate.SelectedDate = DateTime.Today;
            MovieDate.DisplayDate = DateTime.Today;

            selectedLocation = Location.Text; // Get the initially selected location
            selectedDate = MovieDate.Text; // Get the initially set date

            SetMainsContent();
        }
        
        /// On location drop down close check if the location has changed
        /// If so reacret the Main Frame with new location
        private void Location_DropDownClosed(object sender, EventArgs e)
        {
            if (selectedLocation != ((ComboBox)sender).Text)
            {
                selectedLocation = ((ComboBox)sender).Text;
                SetMainsContent();
            }
        }

        /// On date picker window close check if the date has changed
        /// If so recreate the Main Frame with new date
        private void MovieDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (selectedDate != ((DatePicker)sender).Text)
            {
                selectedDate = ((DatePicker)sender).Text;
                SetMainsContent();
            }

        }

        private void SetMainsContent()
        {
            Main.Content = new MainPage();
        }
    }
}
