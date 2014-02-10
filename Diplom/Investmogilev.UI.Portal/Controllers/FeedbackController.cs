using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Investmogilev.UI.Portal.Models;
using Octokit;
using Octokit.Internal;

namespace Investmogilev.UI.Portal.Controllers
{
	public class FeedbackController : Controller
	{
		private readonly string _productName;
		private readonly string _userName;
		private readonly string _userPass;

		public FeedbackController()
		{
			_productName = WebConfigurationManager.AppSettings["gitRepoName"];
			_userName = WebConfigurationManager.AppSettings["gitUserName"];
			_userPass = WebConfigurationManager.AppSettings["gitUserPass"];
		}

		[HttpGet]
		public ActionResult CreateIssue(string baseUri)
		{
			return PartialView(new IssueViewModel { BaseUri = baseUri });
		}

		[HttpPost]
		public ActionResult CreateIssue(IssueViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}


			IIssuesClient issues = new IssuesClient(
				new ApiConnection(
					new Connection(
						new ProductHeaderValue(_productName),
						new InMemoryCredentialStore(
							new Credentials(_userName, _userPass)))));
			var iss = new NewIssue(model.Title)
			{
				Body = model.Body
			};
			foreach (var label in model.Labels)
			{
				iss.Labels.Add(label);
			}

			issues.Create(_userName, _productName, iss);
			return Redirect(model.BaseUri);
		}
	}
}
