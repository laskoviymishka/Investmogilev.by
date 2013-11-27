using Invest.Common.Model.ProjectModels;

namespace InvestPortal.Models
{
    public class EditTaskViewModel : Task
    {
        public EditTaskViewModel()
        {
        }

        public EditTaskViewModel(Task task)
        {
            this.CompletedOn = task.CompletedOn;
            this.Description = task.Description;
            this.Title = task.Title;
            this.IsComplete = task.IsComplete;
            this.IsVerifiedComplete = task.IsVerifiedComplete;
            this.Milestone = task.Milestone;
            this.MoreInfo = task.MoreInfo;
            this.SubTask = task.SubTask;
            this.TaskId = task.TaskId;
        }

        public string ProjectId { get; set; }
    }
}