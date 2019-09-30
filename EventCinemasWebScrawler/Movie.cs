using System.Collections.Generic;

namespace EventCinemasWebScrawler
{
    public class Movie
    {
        public string Name { get; set; }
        
        public string AgeRating { get; set; }

        public int SessionCount { get; set; }

        public List<Session> Sessions { get; set; }
    }
}
