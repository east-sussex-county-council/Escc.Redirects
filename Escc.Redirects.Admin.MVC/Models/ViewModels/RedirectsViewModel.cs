using Escc.Redirects.Admin.MVC.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.Redirects.Admin.MVC.Models.ViewModels
{
    public class RedirectsViewModel
    {
        public TableModel RedirectsTable { get; set; }
        public int Total { get; set; }
        public int ShowResultsFrom { get; set; }
        public string ErrorMessage { get; set; }
        public string Alert { get; set; }
        public string Query { get; set; }
        public bool Search { get; set; }
        public int Type { get; set; }
        public String RedirectTypeString { get; set; }
        public string TypeInfoText { get; set; }

        public RedirectsViewModel()
        {
            Search = false;
            ShowResultsFrom = 0;
            RedirectsTable = new TableModel("RedirectsTable");
        }
    }
}