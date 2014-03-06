using System;
using System.ComponentModel.DataAnnotations;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.User
{
	public class NotificationQueue : IMongoEntity
	{
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

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}