using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Model
{
    public class UnUsedBuilding : Project
    {
        public double Area { get; set; }
        public bool IsSell { get; set; }
        public bool IsCommunicate { get; set; }
        public Nullable<double> BalancePrice { get; set; }
    }
}
