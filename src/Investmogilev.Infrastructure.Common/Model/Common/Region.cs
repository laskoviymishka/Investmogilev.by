// // -----------------------------------------------------------------------
// // <copyright file="Region.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using System.Collections.Generic;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	#endregion

	public class Region : IMongoEntity
	{
		public string RegionName { get; set; }

		public string EnglishName { get; set; }

		public IList<AdditionalInfo> MoreInfo { get; set; }

		public IList<Parametrs> Parametrs { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}