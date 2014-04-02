// // -----------------------------------------------------------------------
// // <copyright file="AdditionalInfo.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using System.ComponentModel.DataAnnotations;
	using Investmogilev.Infrastructure.Common.Localization;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	#endregion

	[BsonKnownTypes(typeof (VideoAdditionalInfo),
		typeof (ImageAdditionalInfo),
		typeof (DocumentAdditionalInfo),
		typeof (LinkAdditionalInfo))]
	public class AdditionalInfo : IMongoEntity
	{
		[Required]
		[Display(ResourceType = typeof (LocalizationResource), Name = "AdditionalInfo_InfoName_Имя")]
		public string InfoName { get; set; }


		[Required]
		[Display(ResourceType = typeof (LocalizationResource), Name = "AdditionalInfo_InfoValue_Описание")]
		public string InfoValue { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}