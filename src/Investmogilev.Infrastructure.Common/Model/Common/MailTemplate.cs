// // -----------------------------------------------------------------------
// // <copyright file="MailTemplate.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using Investmogilev.Infrastructure.Common.State;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	#endregion

	public class MailTemplate : IMongoEntity
	{
		public string Title { get; set; }

		public string Body { get; set; }

		public ProjectWorkflow.Trigger Trigger { get; set; }

		public UserType UserType { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}