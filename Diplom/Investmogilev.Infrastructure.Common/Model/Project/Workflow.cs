using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.State;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	[BsonIgnoreExtraElements]
	public class Workflow
	{
		[Display(Name = "История изменения")]
		public virtual List<History> History { get; set; }

		[Display(Name = "Текущее состояние")]
		public ProjectWorkflow.State CurrentState { get; set; }

		public override string ToString()
		{
			return EnumDescription.GetEnumDescription(CurrentState);
		}
	}
}