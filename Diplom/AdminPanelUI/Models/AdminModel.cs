using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelUI.Models
{
    public class UserAdmin
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string[] Roles { get; set; }
    }
}