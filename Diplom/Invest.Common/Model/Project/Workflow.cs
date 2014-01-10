using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using Invest.Common.State;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Invest.Common.Model.Project
{
    [BsonIgnoreExtraElements]
    public class Workflow
    {
        [Display(Name = "История изменения")]
        public List<History> History { get; set; }

        [Display(Name = "Текущее состояние")]
        public ProjectWorkflow.State CurrentState { get; set; }

        public override string ToString()
        {
            return EnumDescription.GetEnumDescription(CurrentState);
        }
    }
}
