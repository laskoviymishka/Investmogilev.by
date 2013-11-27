using System;

namespace Invest.Common.Model.ProjectModels
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
