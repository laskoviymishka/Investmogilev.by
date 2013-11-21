using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Invest.Common.Model
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

        [BsonIgnore]
        public WorkflowEntity WorkflowState
        {
            get
            {
                if (RepositoryContext.Current.GetOne<WorkflowEntity>(w => w.ProjectId == _id) == null)
                {
                    WorkflowEntity we = new WorkflowEntity();
                    we.CurrenState = "Open";
                    we.ProjectId = _id;
                    we.ChangeHistory = new List<History>();
                    RepositoryContext.Current.Add(we);
                }

                return RepositoryContext.Current.GetOne<WorkflowEntity>(w => w.ProjectId == _id);
            }
        }
    }
}
