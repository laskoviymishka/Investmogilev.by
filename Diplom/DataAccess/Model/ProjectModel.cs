using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryID { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Lng { get; set; }
    }
}
