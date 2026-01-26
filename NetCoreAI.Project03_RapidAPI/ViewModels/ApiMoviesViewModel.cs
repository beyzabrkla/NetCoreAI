using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project03_RapidAPI.ViewModels
{
    public class ApiMoviesViewModel
    {
        public string primaryTitle { get; set; }
        public string releaseDate { get; set; }
        public string[] interests { get; set; }
        public string type { get; set; }
    }
}

//public class Rootobject
//{
//    public Class1[] Property1 { get; set; }
//}

//public class Class1
//{
//    public string id { get; set; }
//    public string url { get; set; }
//    public string primaryTitle { get; set; }
//    public string originalTitle { get; set; }
//    public string type { get; set; }
//    public string description { get; set; }
//    public string primaryImage { get; set; }
//    public Thumbnail[] thumbnails { get; set; }
//    public string trailer { get; set; }
//    public string contentRating { get; set; }
//    public int startYear { get; set; }
//    public object endYear { get; set; }
//    public string releaseDate { get; set; }
//    public string[] interests { get; set; }
//    public string[] countriesOfOrigin { get; set; }
//    public string[] externalLinks { get; set; }
//    public string[] spokenLanguages { get; set; }
//    public string[] filmingLocations { get; set; }
//    public Productioncompany[] productionCompanies { get; set; }
//    public long? budget { get; set; }
//    public long? grossWorldwide { get; set; }
//    public string[] genres { get; set; }
//    public bool isAdult { get; set; }
//    public int runtimeMinutes { get; set; }
//    public float averageRating { get; set; }
//    public int numVotes { get; set; }
//    public int? metascore { get; set; }
//}

//public class Thumbnail
//{
//    public string url { get; set; }
//    public int width { get; set; }
//    public int height { get; set; }
//}

//public class Productioncompany
//{
//    public string id { get; set; }
//    public string name { get; set; }
//}
