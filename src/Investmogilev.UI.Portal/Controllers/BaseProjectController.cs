using System;
using System.Collections.Generic;
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

		private readonly string _descriptionTemplate = @"<p><span style='font-weight: bold; font-size: large;'>Инженерная и транспортная инфраструктура:</span></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Водоснабжение:</span><br></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Водоотведение:</span><br></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Электроснабжение:</span><br></p><p style='font-size: 10pt;'></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Газоснабжение:</span><br></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Подъездные пути:</span><br></p><p style='font-size: 10pt;'><span style='text-decoration: underline;'>Телефонизация:</span><br></p><p><span style='font-weight: bold; font-size: large;'>Характеристики площадки:</span></p><p style='font-size: 10pt;'></p><div><p style='font-size: 10pt;'><span style='text-decoration: underline; font-size: 10pt;'>Категория и вид земельного участка:</span></p><p style='font-size: 10pt;'></p><p><span style='font-size: small; text-decoration: underline;'>Наличие на территории зданий и сооружений:<br></span></p><p style='font-size: 10pt;'></p><p style='font-size: 10pt;'></p></div>";

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
			return View(RepositoryContext.Current.All<Project>());
		}

		#endregion

		#region Create Project

		public ActionResult CreateGreenFieldProject()
		{
			return PartialView(new GreenField { Description = _descriptionTemplate });
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateGreenFieldProject(GreenField model)
		{
			if (ModelState.IsValid)
			{
				ProjectStateManager.StateManagerFactory(model, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).CreateProject(model);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		public ActionResult CreateTemplateProject()
		{
			return PartialView(new Template { Description = _descriptionTemplate });
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateTemplateProject(Template model)
		{
			if (ModelState.IsValid)
			{
				ProjectStateManager.StateManagerFactory(model, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).CreateProject(model);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		public ActionResult CreateUnUsedBuildingProject()
		{
			return PartialView(new UnUsedBuilding());
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateUnUsedBuildingProject(UnUsedBuilding model)
		{
			if (ModelState.IsValid)
			{
				ProjectStateManager.StateManagerFactory(model, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).CreateProject(model);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
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
			return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id) as GreenField);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult GreenFieldProject(GreenField model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id) as GreenField;
				if (initial == null)
				{
					throw new ArgumentNullException("cannot find project");
				}
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.IsInList = model.IsInList;
				initial.Region = model.Region;
				initial.Area = model.Area;
				initial.CadastrValue = model.CadastrValue;
				initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
				initial.Tags = model.Tags;

				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult TemplateProject(Template model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id) as Template;
				if (initial == null)
				{
					throw new ArgumentNullException("cannot find project");
				}
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.IsInList = model.IsInList;
				initial.Region = model.Region;
				initial.Area = model.Area;
				initial.CadastrValue = model.CadastrValue;
				initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
				initial.Tags = model.Tags;

				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		public ActionResult TemplateProject(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id) as Template);
		}

		public ActionResult UnUsedBuildingProject(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(p => p._id == id) as UnUsedBuilding);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult UnUsedBuildingProject(UnUsedBuilding model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id) as UnUsedBuilding;
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.Region = model.Region;
				initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
				initial.Area = model.Area;
				initial.BalancePrice = model.BalancePrice;
				initial.IsExclusion = model.IsExclusion;
				initial.IsRent = model.IsRent;
				initial.AreaBuilding = model.AreaBuilding;
				initial.Tags = model.Tags;
				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		public ActionResult FillProject(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(p => p._id == id));
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult FillProject(Project model)
		{
			if (ModelState.IsValid)
			{
				var initial = RepositoryContext.Current.GetOne<Project>(t => t._id == model._id);
				initial.Name = model.Name;
				initial.Description = model.Description;
				initial.AddressName = model.AddressName;
				initial.Region = model.Region;
				initial.Address = new Address { Lat = model.Address.Lat, Lng = model.Address.Lng };
				initial.Tags = model.Tags;
				ProjectStateManager.StateManagerFactory(initial, User.Identity.Name,
					Roles.GetRolesForUser(User.Identity.Name)).FillInformation(initial);
				return RedirectToAction("Project", "BaseProject", new { id = model._id });
			}

			return View(model);
		}

		public ActionResult Project(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(p => p._id == id));
		}

		public ActionResult Delete(string id)
		{
			RepositoryContext.Current.Delete<Project>(p => p._id == id);
			return RedirectToAction("All", "BaseProject");
		}

		#endregion

		#region Workflow

		public ActionResult WorkFlowForProject(string id)
		{
			return View(RepositoryContext.Current.GetOne<Project>(pr => pr._id == id));
		}

		public ActionResult WorkFlowForProjectPartial(string id)
		{
			return PartialView(RepositoryContext.Current.GetOne<Project>(pr => pr._id == id));
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
					return RepositoryContext.Current.All<Project>(p => p.InvestorUser == User.Identity.Name);
				}

				return RepositoryContext.Current.All<Project>();
			}
		}

		private void BindUsersAndRegions()
		{
			ViewBag.Users = new List<NestedUserViewModel>();
			foreach (Users mongoUser in _mongoRepository.All<Users>())
			{
				ViewBag.Users.Add(new NestedUserViewModel { Name = mongoUser.Username });
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