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
		public virtual AdditionalInfo Info { get; set; }
		public string Id { get; set; }
	}
}