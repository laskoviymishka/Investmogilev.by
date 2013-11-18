using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Model
{
    public class BrownField : Project
    {
        public System.DateTime RegistrationDate { get; set; }
        public double InvestAmount { get; set; }
        public int WorkPlaces { get; set; }
        public Nullable<System.DateTime> EstimateRelease { get; set; }
        public string Appendix { get; set; }
    }
}
