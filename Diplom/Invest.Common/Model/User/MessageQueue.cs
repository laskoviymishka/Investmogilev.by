using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.User
{
    public class MessageQueue : IMongoEntity
    {
        private ObjectId _objectId;

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id
        {
            get { return _objectId.ToString(); }
            set { _objectId = ObjectId.Parse(value); }
        }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        public MessageBody Body { get; set; }

        [Required]
        [Display(Name = "время отправления")]
        public DateTime SendedDate { get; set; }

        [Required]
        [Display(Name = "время получения")]
        public DateTime ReciveTime { get; set; }

        [Required]
        [Display(Name = "Отправитель")]
        public string From { get; set; }

        public IList<string> Cc { get; set; }

        [Required]
        [Display(Name = "Отправить получателю?")]
        public bool IsSended { get; set; }

        [Required]
        [Display(Name = "Прочитано?")]
        public bool IsReaded { get; set; }

        [Required]
        [Display(Name = "Адресат")]
        public string To { get; set; }

        [Required]
        [Display(Name = "Тип сообщения")]
        public MessageType Type { get; set; }
    }
}