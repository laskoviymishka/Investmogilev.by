using System;

namespace Invest.Common.Model.ProjectModels
{
    public class GreenField : Project
    {
        public string InvestNumber { get; set; }
        public System.DateTime EstimateInvestDate { get; set; }
        public string Goal { get; set; }
        public double Area { get; set; }
        public double CadastrValue { get; set; }
        public string ThirdPartiePretender { get; set; }
        public string Infrastructure { get; set; }
        public string OwnerCity { get; set; }
        public string Owner { get; set; }
        public Nullable<int> InvestRequest { get; set; }
        public string Appendix { get; set; }

    }
}
