using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	public class TaskTemplate : IMongoEntity
	{
		public string Title { get; set; }
		public TaskTypes Type { get; set; }
		public ProjectWorkflow.State Step { get; set; }
		public string Body { get; set; }
		public AdditionalInfo Info { get; set; }
		public string _id { get; set; }
	}
}