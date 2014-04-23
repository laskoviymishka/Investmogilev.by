// // -----------------------------------------------------------------------
// // <copyright file="StatSession.cs"  company="One Call Care Management, Inc.">
// // Copyright (c) One Call Care Management, Inc. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------
namespace Investmogilev.Infrastructure.Common.Model.Common
{
	using System;
	using System.Collections.Generic;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	public class StatSession : IMongoEntity
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }

		public DateTime Time { get; set; }
		public List<Activity> Activities { get; set; }
	}
}