using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public class Region : IMongoEntity
	{
		public string RegionName { get; set; }

		public string EnglishName { get; set; }

		public IList<AdditionalInfo> MoreInfo { get; set; }

		public IList<Parametrs> Parametrs { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
	}
}