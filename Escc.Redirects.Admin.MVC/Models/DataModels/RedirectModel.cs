namespace Escc.Redirects.Admin.MVC.Models.DataModels
{
    // Model to hold basic data about a Redirect from the Redirects Database
    public class RedirectModel
    {
        public int RedirectID { get; set; }
        public string Pattern { get; set; }
        public int Type { get; set; }
        public string Comment { get; set; }
        public string Destination { get; set; }
        public RedirectModel(int redirectID, string pattern, int type, string comment, string destination)
        {
            RedirectID = redirectID;
            Pattern = pattern;
            Type = type;
            Comment = comment;
            Destination = destination;
        }
    }
}