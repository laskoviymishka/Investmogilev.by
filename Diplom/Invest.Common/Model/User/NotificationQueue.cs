using System;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.User
{
    public class NotificationQueue : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [Display(Name = "Автор")]
        public string UserName { get; set; }

        [Display(Name = "Заголовок")]
        public string NotificationTitle { get; set; }

        [Display(Name = "Основное сообщение")]
        public string NotigicationBody { get; set; }

        [Display(Name = "Прочитано?")]
        public bool IsRead { get; set; }

        [Display(Name = "Время уведемление")]
        public DateTime NotificationTime { get; set; }
    }
}