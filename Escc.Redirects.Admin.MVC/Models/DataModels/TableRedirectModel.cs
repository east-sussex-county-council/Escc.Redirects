using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.Redirects.Admin.MVC.Models.DataModels
{
    public class TableRedirectModel
    {
        public int ID { get; set;}
        public string URL { get; set; }
        public string Destination { get; set; }
        public string Comment { get; set; }

        public TableRedirectModel(int id, string url, string destination, string comment)
        {
            ID = id;
            URL = url;
            Destination = destination;
            Comment = comment;
        }
    }
}