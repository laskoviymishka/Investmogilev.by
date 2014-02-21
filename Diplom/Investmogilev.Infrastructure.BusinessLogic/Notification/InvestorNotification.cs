using System;
using System.Web.Security;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Model.User;
using Investmogilev.Infrastructure.Common.State;

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	public class InvestorNotification : BaseNotificate, IInvestorNotification
	{
		public InvestorNotification()
			: base(RepositoryContext.Current)
		{
		}

		public void InvestorResponsed(Project currentProject)
		{
			SendMailFromDb(currentProject, currentProject, ProjectWorkflow.Trigger.InvestorResponsed, UserType.Investor);
		}

		public void DocumentUpdate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.DocumentUpdate, UserType.Investor);
		}

		public void ProjectAproved(Project project)
		{
			string pass = Guid.NewGuid().ToString().Substring(0, 5);
			project.InvestorUser = null;
			string login = project.Responses.Find(i => i.IsVerified).InvestorEmail;
			Membership.CreateAccount(login, pass);
			Roles.AddUserToRole(login, "Investor");
			project.Investor = Repository.GetOne<Users>(u => u.Username == login);
			Repository.Update(project);

			SendMailFromDb(project, new {Pass = pass, Login = login, Project = project},
				ProjectWorkflow.Trigger.InvestorSelected, UserType.Investor);
		}


		public void InvolvedOrganizationUpdate(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.InvolvedOrganizationUpdate, UserType.Investor);
		}


		public void Comission(Comission comission, Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.Comission, UserType.Investor);
		}


		public void WaitComission(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToComission, UserType.Investor);
		}


		public void UpdateComissionFix(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ComissionFixUpdate, UserType.Investor);
		}

		public void ComissionFixNeeded(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ComissionFix, UserType.Investor);
		}


		public void WaitIspolcom(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToIspolcom, UserType.Investor);
		}


		public void OnIspolcom(Comission comission, Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.Ispolcom, UserType.Investor);
		}


		public void IspolcomFixNeeded(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToIspolcomFix, UserType.Investor);
		}

		public void UpdateIspolcomFix(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.IspolcomFixUpdate, UserType.Investor);
		}


		public void InMinEconomy(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.ToMinEconomy, UserType.Investor);
		}


		public void MinEconomyResponsed(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.MinEconomyResponsed, UserType.Investor);
		}


		public void Realization(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.UpdateRealization, UserType.Investor);
		}


		public void Done(Project project)
		{
			SendMailFromDb(project, project, ProjectWorkflow.Trigger.UpdateRealization, UserType.Investor);
		}
	}
}