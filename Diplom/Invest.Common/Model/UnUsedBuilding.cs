using System;

namespace Invest.Common.Model
{
    public class UnUsedBuilding : Project
    {
        public double Area { get; set; }
        public bool IsSell { get; set; }
        public bool IsCommunicate { get; set; }
        public Nullable<double> BalancePrice { get; set; }
    }
}
