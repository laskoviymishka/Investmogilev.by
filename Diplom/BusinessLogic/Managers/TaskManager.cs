using System.Collections.Generic;
using System.Linq;
using Invest.Common.Model.Common;
using Invest.Common.Model.ProjectModels;
using MongoDB.Bson;
using MongoRepository;

namespace BusinessLogic.Managers
{
    public class TaskManager
    {
        public IEnumerable<Project> UserProjects(string userName)
        {
            return RepositoryContext.Current.All<Project>(p => p.AssignUser.ToLower() == userName.ToLower());
        }

        public IEnumerable<Project> InvestorProjects(string investorUserName)
        {
            return RepositoryContext.Current.All<Project>(p => p.AssignUser.ToLower() == investorUserName.ToLower());
        }

        public Project GetProject(string id)
        {
            return RepositoryContext.Current.GetOne<Project>(p => p._id == id);
        }

        public Task GetTask(string taskId, string projectId)
        {
            return HandleTreeItems(GetProject(projectId).Tasks, taskId);
        }

        public void UpdateTask(Task task, string projectId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var initialTask = HandleTreeItems(project.Tasks, task.TaskId);
            initialTask.CompletedOn = task.CompletedOn;
            initialTask.Description = task.Description;
            initialTask.Title = task.Title;
            initialTask.IsComplete = task.IsComplete;
            initialTask.IsVerifiedComplete = task.IsVerifiedComplete;
            initialTask.Milestone = task.Milestone;
            RepositoryContext.Current.Update(project);
        }

        public bool CreateTask(Task task, string projectId, string parentId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            if (projectId == parentId)
            {
                if (project.Tasks == null)
                {
                    project.Tasks = new List<Task>();
                }
                project.Tasks.Add(task);
                RepositoryContext.Current.Update(project);
            }
            else
            {
                var parentTask = HandleTreeItems(project.Tasks, parentId);
                if (parentTask != null)
                {
                    if (parentTask.SubTask == null)
                    {
                        parentTask.SubTask = new List<Task>();
                    }
                    parentTask.SubTask.Add(task);
                }
                else
                {
                    return false;
                }

                RepositoryContext.Current.Update(project);
            }

            return true;
        }

        public void DocumentUplaod(string projectId,
            string taskId,
            string infoId,
            string fileName,
            string physicalPath)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var task = HandleTreeItems(project.Tasks, taskId);
            if (task.MoreInfo == null)
            {
                task.MoreInfo = new List<AdditionalInfo>();
            }
            if (task.MoreInfo.Any(i => i._id == infoId && i.InfoName == fileName))
            {
                var info = task.MoreInfo.FirstOrDefault(i => i._id == infoId && i.InfoName == fileName) as DocumentAdditionalInfo;
                if (info != null)
                {
                    info.FilePath = physicalPath;
                }
            }
            else
            {
                task.MoreInfo.Add(new DocumentAdditionalInfo() { _id = infoId, FilePath = physicalPath, InfoName = fileName });
            }

            RepositoryContext.Current.Update(project);
        }

        public void DocumentRemove(string projectId,
            string taskId,
            string infoId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var task = HandleTreeItems(project.Tasks, taskId);
            if (task.MoreInfo != null && task.MoreInfo.Any(i => i._id == infoId))
            {
                var info = task.MoreInfo.FirstOrDefault(i => i._id == infoId) as DocumentAdditionalInfo;
                if (info != null && System.IO.File.Exists(info.FilePath))
                {
                    System.IO.File.Delete(info.FilePath);
                    task.MoreInfo.Remove(info);
                    RepositoryContext.Current.Update(project);
                }
            }
        }

        public DocumentAdditionalInfo DocumentForTaks(string projectId,
            string taskId,
            string infoId)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var task = HandleTreeItems(project.Tasks, taskId);
            return task.MoreInfo.FirstOrDefault(i => i._id == infoId) as DocumentAdditionalInfo;
        }

        public void UpdateDocument(string projectId,
            string taskId,
            string infoId,
            DocumentAdditionalInfo info)
        {
            var project = RepositoryContext.Current.GetOne<Project>(p => p._id == projectId);
            var task = HandleTreeItems(project.Tasks, taskId);
            var initialInfo = task.MoreInfo.FirstOrDefault(i => i._id == infoId) as DocumentAdditionalInfo;
            if (initialInfo != null)
            {
                initialInfo.InfoValue = info.InfoValue;
                RepositoryContext.Current.Update(project);
            }
        }

        #region Private Helper

        private Task HandleTreeItems(IEnumerable<Task> nodes, string id)
        {
            foreach (Task node in nodes)
            {
                Task parentNode;
                if (node.TaskId == id)
                {
                    parentNode = node;
                    return parentNode;
                }
                if (node.SubTask == null || node.SubTask.Count <= 0) continue;
                parentNode = HandleTreeItems(node.SubTask, id);
                if (parentNode != null)
                {
                    return parentNode;
                }
            }

            return null;
        }
        #endregion
    }
}