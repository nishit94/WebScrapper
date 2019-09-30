using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;

namespace EventCinemasWebScrawler
{
    public class MovieWebCrawler
    {
        private int CinemaCode { get; set; }
        private DateTime Date { get; set; }
        private string Url { get; set; }

        // Collection of movies scrapped from event cinemas website
        private ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set { movies = value; }
        }
        
        /// MovieCrawler Class Constructor
        public MovieWebCrawler(string cinemaName, DateTime? date)
        {
            CinemaCode = GetCinemaCode(cinemaName);
            Date = date ?? DateTime.Today;
            Url = string.Format("https://www.eventcinemas.com.au/Sessions#cinemas=" + CinemaCode + "&date={0}", Date.ToString("yyyy-MM-dd"));
        }

        /// Return the cinema code for the following cinemas.
        private int GetCinemaCode(string cinemaName)
        {
            Dictionary<string, int> CinemaNameCode = new Dictionary<string, int>()
            {
                { "Brisbane Meyer Center", 59 },
                { "Browns Plain", 29 },
                { "Carindale", 28 },
                { "Chermside", 56 },
                { "Garden City", 49 },
                { "Indooroopilly", 48 },
                { "Springfield", 83 }
            };

            return CinemaNameCode[cinemaName];
        }

        /// <summary>
        /// Main function that gets movie information from the even cinemas website.
        /// Uses an instance of chrome web browser to run the dynamic jQuery scripts before scrapping the information.
        /// Stores this information in Movies collection list.
        /// </summary>
        public void GetMovieInformation()
        {
            movies = new ObservableCollection<Movie>();

            try
            {
                using (var driver = new ChromeDriver())
                {
                    driver.Navigate().GoToUrl(Url);
                    var doc = new HtmlDocument();
                    doc.LoadHtml(driver.PageSource);

                    var items = doc.DocumentNode.SelectNodes("//*[@class = 'movie-list-item movie-container-item']");
                    if (items == null) throw new Exception("No movies found, please try again.");
                    foreach (var item in items)
                    {
                        string name = HttpUtility.HtmlDecode(item.SelectSingleNode(".//span[@class = 'title']")?.InnerText) ?? "N/A";
                        string rating = HttpUtility.HtmlDecode(item.SelectSingleNode(".//span[@class = 'MA15 rating' or @class = 'PG rating' or @class = 'M rating' or @class = 'R18 rating' or @class = 'E rating']")?.InnerText) ?? "N/A";
                        List<Session> movieSessions = new List<Session>();

                        var sessions = item.SelectNodes(".//a[@class = 'session-btn']");
                        if (sessions == null) throw new Exception("No Sessions found, please try again.");
                        foreach (var session in sessions)
                        {
                            string type = HttpUtility.HtmlDecode(session.SelectSingleNode(".//span[@class = 'screen-type']")?.InnerText) ?? "N/A";
                            string time = HttpUtility.HtmlDecode(session.SelectSingleNode(".//span[@class = 'time']")?.InnerText) ?? "N/A";
                            movieSessions.Add(new Session() { Type = type, Time = time });
                        }

                        movies.Add(new Movie { Name = name, AgeRating = rating, SessionCount = movieSessions.Count, Sessions = movieSessions });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
