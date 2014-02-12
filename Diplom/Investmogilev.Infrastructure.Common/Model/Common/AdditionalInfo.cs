using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Localization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
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