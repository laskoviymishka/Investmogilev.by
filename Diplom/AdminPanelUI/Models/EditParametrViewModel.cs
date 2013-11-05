using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelUI.Models
{
    public class EditParametrViewModel
    {
        public string RegionId { get; set; }
        public string ParametrName { get; set; }
        public List<KeyValuePair<int, double>> Values { get; set; }
    }
}