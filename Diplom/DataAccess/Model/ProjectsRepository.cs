using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ProjectsRepository
    {
        #region Fields

        private readonly dbRegionsEntities _dataBase;

        #endregion

        #region Constructor

        public ProjectsRepository(dbRegionsEntities dataBase)
        {
            _dataBase = dataBase;
        }

        #endregion

        #region Getters

        public IList<ProjectModel> GetAllProjects()
        {
            IList<ProjectModel> result = new List<ProjectModel>();
            foreach(ProjectDTO project in _dataBase.Projects)
            {
                ProjectModel projectModel = new ProjectModel();
                projectModel.Address = project.Address;
                projectModel.CategoryID = project.CategoryID;
                projectModel.Description = project.Description;
                projectModel.Lat = project.Lat;
                projectModel.Lng = project.Lng;
                projectModel.Name = project.Name;
                projectModel.ProjectID = project.ProjectID;
                projectModel.Region = project.Region;
            }
            return result;
        }

        #endregion

    }
}
