using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
    [BsonKnownTypes(typeof(VideoAdditionalInfo),
        typeof(ImageAdditionalInfo),
        typeof(DocumentAdditionalInfo),
        typeof(LinkAdditionalInfo))]
    public class AdditionalInfo : IMongoEntity
    {
        [Required]
        [Display(Name = "Имя")]
        public string InfoName
        {
            get;
            set;
        }


        [Required]
        [Display(Name = "Описание")]
        public string InfoValue
        {
            get;
            set;
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; }
    }
}
