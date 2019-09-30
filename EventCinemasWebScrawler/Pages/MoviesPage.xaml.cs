using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EventCinemasWebScrawler.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private MovieWebCrawler crawler;

        /// Initialising the Movies Page
        /// Fetching data from EventCinemas website to display in the DataGrid
        public MainPage()
        {
            try
            {
                InitializeComponent();

                string location = ((MainWindow)Application.Current.MainWindow).Location.Text;
                DateTime? date = ((MainWindow)Application.Current.MainWindow).MovieDate.SelectedDate;

                crawler = new MovieWebCrawler(location, date);
                DataContext = crawler;
                crawler.GetMovieInformation();

                MoviesDataGrid.ItemsSource = crawler.Movies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// DataGrid double click functionality
        /// Creates Session Page displaying session information related to the clicked movie
        private void MoviesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var MainFrame = ((MainWindow)Application.Current.MainWindow).Main;

                SessionsPage page = new SessionsPage();
                List<Session> sessions = ((Movie)((DataGrid)sender).CurrentCell.Item).Sessions;
                page.SessionsDataGrid.ItemsSource = sessions;

                MainFrame.Content = page;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
