using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Invest.Common.Model.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.ProjectModels
{
    public class Task
    {
        private DateTime _milestoneDate;
        private bool _isComplete;

        public string TaskId { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание проекта")]
        public string Description { get; set; }

        public IList<Task> SubTask { get; set; }

        [Required]
        [Display(Name = "Приблизительная дата прохождения контрольной точки")]
        public DateTime Milestone
        {
            get
            {
                if (SubTask != null && SubTask.Count > 0)
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
        public DateTime CompletedOn { get; set; }

        [Required]
        [Display(Name = "Выполнен?")]
        public bool IsComplete {
            get
            {
                if (SubTask != null && SubTask.Count > 0)
                {
                    if (SubTask.Any(task => task.IsComplete == false))
                    {
                        _isComplete = false;
                    }
                    else
                    {
                        _isComplete = true;
                    }
                }

                return _isComplete;
            }
            set { _isComplete = value; }
        }

        [Required]
        [Display(Name = "Проверен администратором?")]
        public bool IsVerifiedComplete { get; set; }

        public IList<AdditionalInfo> MoreInfo
        {
            get;
            set;
        }

        [BsonIgnore]
        [Display(Name = "Опаздывает выполенение?")]
        public bool IsLate {
            get { return (DateTime.Now > _milestoneDate) | (_milestoneDate < CompletedOn); }
        }
    }
}