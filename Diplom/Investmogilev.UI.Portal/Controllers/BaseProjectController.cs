using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Security;
using Investmogilev.Infrastructure.BusinessLogic.Managers;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.Repository;

namespace Investmogilev.UI.Portal.Controllers
{
	public class BaseProjectController : Controller
	{
		#region Private Fields

		private readonly string _descriptionTemplate = @"<p><span style='font-weight: bold; font-size: large;'>Инженерная и транспортная инфраструктура:</span></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Водоснабжение:</span><br><br></p><hr style='font-size: 10pt;'><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Водоотведение:</span><br><br></p><hr style='font-size: 10pt;'><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Электроснабжение:</span><br></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'><br></span></p><hr style='font-size: 10pt;'><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Газоснабжение:</span><br><br></p><hr style='font-size: 10pt;'><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Подъездные пути:</span><br><br></p><hr style='font-size: 10pt;'><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Телефонизация:</span><br><br></p><hr style='font-size: 10pt;'><p><span style='font-weight: bold; font-size: large;'>Характеристики площадки:</span></p><p style='font-size: 10pt;'></p><div><p style='font-size: 10pt;'><span style='text-decoration: underline; font-size: 10pt;'>Категория и вид земельного участка:</span></p><p style='font-size: 10pt;'><br></p><hr style='font-size: 10pt;'><p><span style='font-size: small; text-decoration: underline;'>Наличие на территории зданий и сооружений:<br><br></span></p><hr><p style='font-size: 10pt;'></p><p style='font-size: 10pt;'></p></div>";

		private readonly IRepository _mongoRepository;

		#endregion

		#region Constructor

		public BaseProjectController()
		{
			_mongoRepository = RepositoryContext.Current;
		}

		#endregion

		#region All project

		public ActionResult Index()
		{
			return View(AllProjectForUser);
		}

		public ActionResult All()
		{
			BindUsersAndRegions();
			return View(RepositoryContext.Current.All<Project>().Include(project => project.WorkflowState));
		}

		#endregion

		#region Create Project

		public ActionResult CreateGreenFieldProject()
		{
			return PartialView(new GreenField{Description = _descriptionTemplate});
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateGreenFieldProject(GreenField model)
		{
			if (ModelState.IsValid)
			{
				ProjectStateManager.StateManagerFactory(model, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).CreateProject(model);
				return RedirectToAction("Project", "BaseProject", new {id = model.Id});
			}

			return View(model);
		}

		public ActionResult CreateUnUsedBuildingProject()
		{
			return PartialView();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateUnUsedBuildingProject(UnUsedBuilding model)
		{
			if (ModelState.IsValid)
			{
				ProjectStateManager.StateManagerFactory(model, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).CreateProject(model);
				return RedirectToAction("Project", "BaseProject", new {id = model.Id});
			}

			return View(model);
		}

		public ActionResult CreateProject(string id)
		{
			return View();
		}

		#endregion

		#region Update Project

		public ActionResult GreenFieldProject(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(p => p.Id == id) as GreenField);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult GreenFieldProject(GreenField model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t.Id == model.Id);
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.Region = model.Region;
				initial.Address = new Address {Lat = model.Address.Lat, Lng = model.Address.Lng};
				initial.Tags = model.Tags;

				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new {id = model.Id});
			}

			return View(model);
		}

		public ActionResult UnUsedBuildingProject(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(p => p.Id == id) as UnUsedBuilding);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult UnUsedBuildingProject(UnUsedBuilding model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t.Id == model.Id) as UnUsedBuilding;
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.Region = model.Region;
				initial.Address = new Address {Lat = model.Address.Lat, Lng = model.Address.Lng};
				initial.Area = model.Area;
				initial.BalancePrice = model.BalancePrice;
				initial.IsCommunicate = model.IsCommunicate;
				initial.IsSell = model.IsSell;
				initial.Tags = model.Tags;
				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new {id = model.Id});
			}

			return View(model);
		}

		public ActionResult FillProject(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(p => p.Id == id));
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult FillProject(Project model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t.Id == model.Id);
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.Region = model.Region;
				initial.Address = new Address {Lat = model.Address.Lat, Lng = model.Address.Lng};
				initial.Tags = model.Tags;
				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new {id = model.Id});
			}

			return View(model);
		}

		public ActionResult Project(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(p => p.Id == id));
		}

		public ActionResult Delete(string id)
		{
			RepositoryContext.Current.Delete<Project>(p => p.Id == id);
			return RedirectToAction("All", "BaseProject");
		}

		#endregion

		#region Workflow

		public ActionResult WorkFlowForProject(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(pr => pr.Id == id));
		}

		public ActionResult WorkFlowForProjectPartial(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(pr => pr.Id == id));
		}

		#endregion

		#region Private Helpers

		private IEnumerable<Project> AllProjectForUser
		{
			get
			{
				BindUsersAndRegions();
				if (User.IsInRole("Investor"))
				{
					return RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name).Include(project => project.WorkflowState);
				}

				return RepositoryContext.Current.All<Project>().Include(project => project.WorkflowState);
			}
		}

		private void BindUsersAndRegions()
		{
			ViewBag.Users = new List<NestedUserViewModel>();
			foreach (Users mongoUser in _mongoRepository.All<Users>())
			{
				ViewBag.Users.Add(new NestedUserViewModel {Name = mongoUser.Username});
			}
		}

		public class NestedRegionViewModel
		{
			public string RegionName { get; set; }
		}

		public class NestedUserViewModel
		{
			public string Name { get; set; }
		}

		#endregion
	}
}