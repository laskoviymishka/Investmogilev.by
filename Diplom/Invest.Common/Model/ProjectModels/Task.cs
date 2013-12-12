using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Invest.Common.Model.ProjectModels
{
    public class Task : IMongoEntity
    {
        private DateTime _milestoneDate;
        private bool _isComplete;
        private bool _isVerified;
        private IEnumerable<Task> _subTask;

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string ParentId { get; set; }

        public string AssignUser { get; set; }

        public IList<string> Tags { get; set; }

        public string ProjectId { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [BsonIgnore]
        public IEnumerable<Task> SubTask
        {
            get
            {
                _subTask = RepositoryContext.Current.All<Task>(t => t.ParentId == _id);

                if (_subTask != null)
                {
                    foreach (Task task in _subTask)
                    {
                        if (string.IsNullOrEmpty(task.ProjectId))
                        {
                            task.ProjectId = ProjectId;
                        }
                    }
                }
                return _subTask;
            }
        }

        public IEnumerable<TaskReport> Reports { get; set; }

        [Required]
        [Display(Name = "Приблизительная дата прохождения контрольной точки")]
        public DateTime Milestone
        {
            get
            {
                if (SubTask != null && SubTask.Count() > 0)
                {
                    foreach (var task in SubTask.Where(task => _milestoneDate < task.Milestone))
                    {
                        _milestoneDate = task.Milestone;
                    }
                }

                return _milestoneDate;
            }
            set { _milestoneDate = value; }
        }

        [Display(Name = "Дата выполнения")]
        public DateTime CompletedOn { get; set; }

        [Required]
        [Display(Name = "Выполнен?")]
        public bool IsComplete
        {
            get
            {
                if (SubTask != null && SubTask.Any())
                {
                    _isComplete = SubTask.All(task => task.IsComplete);
                }

                return _isComplete;
            }
            set { _isComplete = value; }
        }

        [Required]
        [Display(Name = "Проверен администратором?")]
        public bool IsVerifiedComplete {
            get
            {
                if (SubTask != null && SubTask.Any())
                {
                    _isVerified = SubTask.All(task => task.IsVerifiedComplete);
                }

                return _isVerified;
            }
            set { _isVerified = value; }
        }

        public IList<AdditionalInfo> MoreInfo
        {
            get;
            set;
        }

        [BsonIgnore]
        [Display(Name = "Опаздывает выполенение?")]
        public bool IsLate
        {
            get { return (DateTime.Now > _milestoneDate) | (_milestoneDate < CompletedOn); }
        }
    }
}