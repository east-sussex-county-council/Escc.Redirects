using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Escc.Redirects.Admin.MVC.Models.DataModels
{
    public class TableModel
    {
        public DataTable Table { get; set; }
        public string ID { get; set; }

        public TableModel(string id)
        {
            ID = id;
        }
    }
}