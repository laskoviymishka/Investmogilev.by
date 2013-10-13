using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class BrownFieldModel : ProjectModel
    {
        public System.DateTime RegistrationDate { get; set; }
        public double InvestAmount { get; set; }
        public int WorkPlaces { get; set; }
        public Nullable<System.DateTime> EstimateRelease { get; set; }
        public string Appendix { get; set; }
        public int ProjectID { get; set; }
    }
}
