namespace SlackOverflow.Models
{
    public class GoogleCustomSearchResponse
    {
        public string kind { get; set; }
        public Url url { get; set; }
        public Queries queries { get; set; }
        public Context context { get; set; }
        public Searchinformation searchInformation { get; set; }
        public Item[] items { get; set; }
    }

    public class Url
    {
        public string type { get; set; }
        public string template { get; set; }
    }

    public class Queries
    {
        public Nextpage[] nextPage { get; set; }
        public Request[] request { get; set; }
    }

    public class Nextpage
    {
        public string title { get; set; }
        public string totalResults { get; set; }
        public string searchTerms { get; set; }
        public int count { get; set; }
        public int startIndex { get; set; }
        public string inputEncoding { get; set; }
        public string outputEncoding { get; set; }
        public string safe { get; set; }
        public string cx { get; set; }
    }

    public class Request
    {
        public string title { get; set; }
        public string totalResults { get; set; }
        public string searchTerms { get; set; }
        public int count { get; set; }
        public int startIndex { get; set; }
        public string inputEncoding { get; set; }
        public string outputEncoding { get; set; }
        public string safe { get; set; }
        public string cx { get; set; }
    }

    public class Context
    {
        public string title { get; set; }
    }

    public class Searchinformation
    {
        public float searchTime { get; set; }
        public string formattedSearchTime { get; set; }
        public string totalResults { get; set; }
        public string formattedTotalResults { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string title { get; set; }
        public string htmlTitle { get; set; }
        public string link { get; set; }
        public string displayLink { get; set; }
        public string snippet { get; set; }
        public string htmlSnippet { get; set; }
        public string cacheId { get; set; }
        public string formattedUrl { get; set; }
        public string htmlFormattedUrl { get; set; }
        public Pagemap pagemap { get; set; }
    }

    public class Pagemap
    {
        public Cse_Image[] cse_image { get; set; }
        public Qapage[] qapage { get; set; }
        public Answer[] answer { get; set; }
        public Question[] question { get; set; }
        public Metatag[] metatags { get; set; }
    }

    public class Cse_Image
    {
        public string src { get; set; }
    }

    public class Qapage
    {
        public string image { get; set; }
        public string primaryimageofpage { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Answer
    {
        public string upvotecount { get; set; }
        public string text { get; set; }
    }

    public class Question
    {
        public string image { get; set; }
        public string name { get; set; }
        public string upvotecount { get; set; }
        public string text { get; set; }
        public string answercount { get; set; }
    }

    public class Metatag
    {
        public string twittercard { get; set; }
        public string twitterdomain { get; set; }
        public string ogtype { get; set; }
        public string ogimage { get; set; }
        public string twittertitle { get; set; }
        public string twitterdescription { get; set; }
        public string ogurl { get; set; }
        public string twitterappcountry { get; set; }
        public string twitterappnameiphone { get; set; }
        public string twitterappidiphone { get; set; }
        public string twitterappurliphone { get; set; }
        public string twitterappnameipad { get; set; }
        public string twitterappidipad { get; set; }
        public string twitterappurlipad { get; set; }
        public string twitterappnamegoogleplay { get; set; }
        public string twitterappurlgoogleplay { get; set; }
        public string twitterappidgoogleplay { get; set; }
    }

}
