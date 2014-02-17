using System.Collections.Generic;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.User
{
	public class Role : IMongoEntity
	{
		public string ApplicationName { get; set; }
		public string UserName { get; set; }
		[BsonIgnore]
		public virtual List<Users> User { get; set; }

		public string Id { get; set; }
	}
}