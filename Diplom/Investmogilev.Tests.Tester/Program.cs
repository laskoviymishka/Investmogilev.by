using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Investmogilev.Infrastructure.BusinessLogic.Notification;
using Investmogilev.Infrastructure.BusinessLogic.Wokflow;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Repository;
using Investmogilev.Infrastructure.Common.Repository.EF;
using Investmogilev.Infrastructure.Common.State;
using Investmogilev.Infrastructure.Common.State.StateAttributes;
using MongoDB.Bson;

namespace Investmogilev.Tests.Tester
{
	class Program
	{
		private static Project _currentProject;
		private static IList _projects;
		private static IRepository _repository = RepositoryContext.Current;
		private static IUserNotification _userNotificationl;
		private static IAdminNotification _adminNotificate;
		private static IInvestorNotification _investorNotificate;
		private static string _userName;
		private static IEnumerable<string> _roles;
		private static ProjectWorkflowWrapper _workflow;
		private static UnitsOfWorkContainer _unitOfWorksContainer;

		static void Main(string[] args)
		{
			//var _currentUser = "";
			//_roles = new List<string>() { "Admin" };
			//_investorNotificate = new InvestorNotification();
			//_adminNotificate = new AdminNotification();
			//_userNotificationl = new UserNotification();
			//_currentProject = new GreenField();
			//var _unitsOfWork = new UnitsOfWorkContainer(_currentProject,
			//	  new MongoRepository("mongodb://178.124.129.147/", "Projects"),
			//	 _userNotificationl,
			//	 _adminNotificate,
			//	 _investorNotificate,
			//	 _currentUser, _roles);

			//if (_currentProject.WorkflowState == null)
			//{
			//	_currentProject.WorkflowState = new Workflow()
			//	{
			//		CurrentState = ProjectWorkflow.State.Open
			//	};
			//}
			////_workflow = new ProjectWorkflowWrapper(new ProjectWorkflow(_currentProject.WorkflowState.CurrentState), _unitsOfWork);
			////_workflow.IsMoveablde(ProjectWorkflow.Trigger.FillInformation);
			//var builder = new AttributeStateMachineBuilder();
			//ProjectStateContext context = new ProjectStateContext();
			//context.AdminNotification = _adminNotificate;
			//context.CurrentProject = _currentProject;
			//context.InvestorNotification = _investorNotificate;
			//context.Repository = _repository;
			//context.Roles = _roles;
			//context.UserName = _currentUser;
			//context.UserNotification = _userNotificationl;
			//var statemachine = builder
			//	.BuilStateMachine<ProjectWorkflow.State, ProjectWorkflow.Trigger>(
			//		"test",
			//		context,
			//		_currentProject.WorkflowState.CurrentState);
			//Console.Write(statemachine.CanFire(ProjectWorkflow.Trigger.FillInformation));
			ProjectDataContext db = new ProjectDataContext();
			db.Projects.Add(new GreenField() {_id = ObjectId.GenerateNewId().ToString()});
			db.SaveChanges();
		}
		//private static void GenerateDependendenciesValues()
		//{
		//    RegionRepository repo = new RegionRepository();
		//    List<Region> regions = repo.GetAll().ToList();
		//    Dictionary<string, Dictionary<int, List<double>>> parRes = new Dictionary<string, Dictionary<int, List<double>>>();
		//    Dictionary<string, Dictionary<int, List<double>>> parDependedRes = new Dictionary<string, Dictionary<int, List<double>>>();
		//    foreach (Region region in regions)
		//    {
		//        foreach (var parentParam in region.Parametrs)
		//        {
		//            if (!parDependedRes.ContainsKey(parentParam.ParametrName))
		//            {
		//                parDependedRes.Add(parentParam.ParametrName, new Dictionary<int, List<double>>());
		//            }

		//            for (int i = 0; i < parentParam.Values.Count; i++)
		//            {
		//                if (!parDependedRes[parentParam.ParametrName].ContainsKey(parentParam.Values[i].Key))
		//                {
		//                    parDependedRes[parentParam.ParametrName].Add(parentParam.Values[i].Key, new List<double>());
		//                    parDependedRes[parentParam.ParametrName][parentParam.Values[i].Key].Add(parentParam.Values[i].Value);
		//                }
		//                else
		//                {
		//                    parDependedRes[parentParam.ParametrName][parentParam.Values[i].Key].Add(parentParam.Values[i].Value);
		//                }

		//            }
		//            foreach (var parametr in parentParam.ChildParametrs)
		//            {
		//                if (!parDependedRes.ContainsKey(parametr.ParametrName))
		//                {
		//                    parDependedRes.Add(parametr.ParametrName, new Dictionary<int, List<double>>());
		//                }
		//                if (!parRes.ContainsKey(parametr.ParametrName))
		//                {
		//                    parRes.Add(parametr.ParametrName, new Dictionary<int, List<double>>());
		//                }

		//                for (int i = 0; i < parametr.AbsoluteValues.Count; i++)
		//                {
		//                    if (!parDependedRes[parametr.ParametrName].ContainsKey(parametr.AbsoluteValues[i].Key))
		//                    {
		//                        parDependedRes[parametr.ParametrName].Add(parametr.AbsoluteValues[i].Key, new List<double>());
		//                        parDependedRes[parametr.ParametrName][parametr.AbsoluteValues[i].Key].Add(parametr.AbsoluteValues[i].Value);
		//                    }
		//                    else
		//                    {
		//                        parDependedRes[parametr.ParametrName][parametr.AbsoluteValues[i].Key].Add(parametr.AbsoluteValues[i].Value);
		//                    }

		//                    if (!parRes[parametr.ParametrName].ContainsKey(parametr.AbsoluteValues[i].Key))
		//                    {
		//                        parRes[parametr.ParametrName].Add(parametr.Values[i].Key, new List<double>());
		//                        parRes[parametr.ParametrName][parametr.Values[i].Key].Add(parametr.Values[i].Value);
		//                    }
		//                    else
		//                    {
		//                        parRes[parametr.ParametrName][parametr.Values[i].Key].Add(parametr.Values[i].Value);
		//                    }
		//                }
		//            }
		//        }
		//    }

		//    Dictionary<string, Dictionary<int, double>> avgValue = new Dictionary<string, Dictionary<int, double>>();
		//    Dictionary<string, Dictionary<int, double>> avgDependedValue = new Dictionary<string, Dictionary<int, double>>();

		//    foreach (var item in parRes.Keys)
		//    {
		//        avgValue.Add(item, new Dictionary<int, double>());
		//        foreach (var dict in parRes[item].Keys)
		//        {
		//            if (parRes[item].ContainsKey(dict))
		//            {
		//                avgValue[item].Add(dict, parRes[item][dict].Average());
		//            }
		//        }
		//    }

		//    foreach (var item in parDependedRes.Keys)
		//    {
		//        avgDependedValue.Add(item, new Dictionary<int, double>());
		//        foreach (var dict in parDependedRes[item].Keys)
		//        {
		//            avgDependedValue[item].Add(dict, parDependedRes[item][dict].Average());
		//        }
		//    }

		//    foreach (Region region in regions)
		//    {
		//        foreach (var parentParam in region.Parametrs)
		//        {
		//            List<KeyValuePair<int, double>> dependAbsoluteValues = new List<KeyValuePair<int, double>>();
		//            List<KeyValuePair<int, double>> dependValues = new List<KeyValuePair<int, double>>();
		//            double parIntegral = 0;
		//            int counetr = 0;
		//            for (int i = 0; i < parentParam.Values.Count; i++)
		//            {
		//                try
		//                {
		//                    dependAbsoluteValues.Add(new KeyValuePair<int, double>(
		//                        parentParam.Values[i].Key,
		//                        parentParam.Values[i].Value / avgDependedValue[parentParam.ParametrName][parentParam.Values[i].Key]));
		//                    if (avgDependedValue[parentParam.ParametrName][parentParam.Values[i].Key] < 1000000000 && avgDependedValue[parentParam.ParametrName][parentParam.Values[i].Key] > 0)
		//                    {
		//                        parIntegral += parentParam.Values[i].Value / avgDependedValue[parentParam.ParametrName][parentParam.Values[i].Key];
		//                        counetr++;
		//                    }

		//                }
		//                catch { }
		//            }
		//            parentParam.IntegralValue = parIntegral / counetr;
		//            parentParam.DependAbsoluteValues = dependAbsoluteValues;
		//            parentParam.DependValues = dependValues;
		//            foreach (var parametr in parentParam.ChildParametrs)
		//            {
		//                List<KeyValuePair<int, double>> childDependAbsoluteValues = new List<KeyValuePair<int, double>>();
		//                List<KeyValuePair<int, double>> childDependValues = new List<KeyValuePair<int, double>>();
		//                double childIntegral = 0;
		//                int counetr1 = 0;
		//                for (int i = 0; i < parametr.AbsoluteValues.Count; i++)
		//                {
		//                    try
		//                    {
		//                        childDependAbsoluteValues.Add(new KeyValuePair<int, double>(
		//                            parametr.Values[i].Key,
		//                            parametr.Values[i].Value / avgDependedValue[parametr.ParametrName][parametr.Values[i].Key]));
		//                        if (avgDependedValue[parametr.ParametrName][parametr.Values[i].Key] < 1000000000 && avgDependedValue[parametr.ParametrName][parametr.Values[i].Key] > 0)
		//                        {
		//                            childIntegral += parametr.AbsoluteValues[i].Value / avgDependedValue[parametr.ParametrName][parametr.Values[i].Key];
		//                            counetr1++;
		//                        }
		//                    }
		//                    catch { }
		//                    try
		//                    {
		//                        childDependValues.Add(new KeyValuePair<int, double>(
		//                            parametr.AbsoluteValues[i].Key,
		//                            parametr.AbsoluteValues[i].Value / avgValue[parametr.ParametrName][parametr.Values[i].Key]));

		//                    }
		//                    catch { }

		//                }
		//                parametr.IntegralValue = childIntegral / counetr1;
		//                parametr.DependAbsoluteValues = childDependAbsoluteValues;
		//                parametr.DependValues = childDependValues;
		//            }
		//        }
		//        Console.WriteLine("update");
		//        //repo.Update<Region>(region);
		//    }
		//}

		//private static void GenerateAbsoluteParametrs()
		//{
		//    RegionRepository repo = new RegionRepository();
		//    List<Region> regions = repo.GetAll().ToList();

		//    foreach (Region region in regions)
		//    {
		//        foreach (var parentParam in region.Parametrs)
		//        {
		//            foreach (var parametr in parentParam.ChildParametrs)
		//            {
		//                List<KeyValuePair<int, double>> absoluteValues = new List<KeyValuePair<int, double>>();
		//                for (int i = 0; i < parametr.Values.Count; i++)
		//                {
		//                    KeyValuePair<int, double> newPar;
		//                    if (i == 0)
		//                    {
		//                        newPar = new KeyValuePair<int, double>(parametr.Values[i].Key, 1);
		//                    }
		//                    else
		//                    {
		//                        newPar = new KeyValuePair<int, double>(parametr.Values[i].Key, parametr.Values[i].Value / parametr.Values[i - 1].Value);
		//                    }
		//                    absoluteValues.Add(newPar);
		//                }
		//                parametr.AbsoluteValues = absoluteValues;
		//            }
		//        }
		//        Console.WriteLine("update");
		//        //repo.Update<Region>(region);
		//    }
		//}

		//private static void WriteGreenField()
		//{

		//    FileStream stream = File.Open(@"d:/Перечень земучастков для заключения ИД 01.10.2013+участки вне перечня.xls", FileMode.Open, FileAccess.Read);

		//    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
		//    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		//    //...
		//    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
		//    DataSet result = excelReader.AsDataSet();

		//    ProjectRepository repo = new ProjectRepository();
		//    var table = result.Tables[0];
		//    for (int i = 6; i <= table.Rows.Count; i++)
		//    {
		//        try
		//        {
		//            GreenField gr = new GreenField();
		//            gr.AddressName = table.Rows[i].Field<string>(3);
		//            gr.Appendix = table.Rows[i].Field<string>(11);
		//            gr.Area = table.Rows[i].Field<double>(5);
		//            gr.CadastrValue = table.Rows[i].Field<double>(6);
		//            gr.Contact = table.Rows[i].Field<string>(9);
		//            gr.Description = table.Rows[i].Field<string>(4);
		//            gr.EstimateInvestDate = DateTime.FromOADate(table.Rows[i].Field<double>(2));
		//            gr.Goal = table.Rows[i].Field<string>(4);
		//            gr.Infrastructure = table.Rows[i].Field<string>(8);
		//            gr.InvestNumber = table.Rows[i].Field<string>(1);
		//            if (table.Rows[i].ItemArray[10].ToString() != "")
		//            {
		//                gr.InvestRequest = (int)table.Rows[i].Field<double>(10);
		//            }
		//            else
		//            {
		//                gr.InvestRequest = 0;
		//            }
		//            gr.Name = table.Rows[i].Field<string>(1);
		//            gr.Owner = table.Rows[i].Field<string>(9);
		//            gr.OwnerCity = "Могилев";
		//            gr.Region = "Могилев";
		//            gr.ThirdPartiePretender = table.Rows[i].Field<string>(7);
		//            gr.Address = new Address() { Lat = 1, Lng = 1 };
		//            Console.WriteLine("insert");
		//            repo.InsertProject(gr);
		//        }
		//        catch
		//        {
		//            Console.WriteLine("fail");
		//        }
		//    }

		//    //6. Free resources (IExcelDataReader is IDisposable)
		//    excelReader.Close();
		//}

		//private static void WriteBrownField()
		//{
		//    FileStream stream = File.Open(@"d:/Перечень обращений по инвестдоговорам 2013.xls", FileMode.Open, FileAccess.Read);

		//    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
		//    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		//    //...
		//    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
		//    DataSet result = excelReader.AsDataSet();

		//    ProjectRepository repo = new ProjectRepository();
		//    var table = result.Tables["Принятые решения ОИК "];
		//    for (int i = 8; i <= table.Rows.Count; i++)
		//    {
		//        try
		//        {
		//            BrownField br = new BrownField();
		//            br.Contact = table.Rows[i].Field<string>(1);
		//            br.Description = table.Rows[i].Field<string>(2);
		//            br.Name = table.Rows[i].Field<string>(8);
		//            br.EstimateRelease = DateTime.FromOADate(table.Rows[i].Field<double>(11));
		//            br.InvestAmount = table.Rows[i].Field<double>(9);
		//            br.RegistrationDate = DateTime.FromOADate(table.Rows[i].Field<double>(3));
		//            br.Region = "";
		//            br.WorkPlaces = (int)table.Rows[i].Field<double>(10);
		//            br.AddressName = table.Rows[i].Field<string>(2);
		//            br.Address = new Address() { Lat = 1, Lng = 1 };
		//            Console.WriteLine("asd");
		//            repo.InsertProject(br);
		//        }
		//        catch { }
		//    }

		//    //6. Free resources (IExcelDataReader is IDisposable)
		//    excelReader.Close();
		//}
	}
}
