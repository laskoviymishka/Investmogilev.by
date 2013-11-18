using MongoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tester
{
    class Program
    {
        public enum ProcessState
        {
            Inactive,
            Active,
            Paused,
            Terminated
        }

        public enum Command
        {
            Begin,
            End,
            Pause,
            Resume,
            Exit
        }

        public class Process
        {
            class StateTransition
            {
                readonly ProcessState CurrentState;
                readonly Command Command;

                public StateTransition(ProcessState currentState, Command command)
                {
                    CurrentState = currentState;
                    Command = command;
                }

                public override int GetHashCode()
                {
                    return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    StateTransition other = obj as StateTransition;
                    return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
                }
            }

            Dictionary<StateTransition, ProcessState> transitions;
            public ProcessState CurrentState { get; private set; }

            public Process()
            {
                CurrentState = ProcessState.Inactive;
                transitions = new Dictionary<StateTransition, ProcessState>
                    {
                        { new StateTransition(ProcessState.Inactive, Command.Exit), ProcessState.Terminated },
                        { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active },
                        { new StateTransition(ProcessState.Active, Command.End), ProcessState.Inactive },
                        { new StateTransition(ProcessState.Active, Command.Pause), ProcessState.Paused },
                        { new StateTransition(ProcessState.Paused, Command.End), ProcessState.Inactive },
                        { new StateTransition(ProcessState.Paused, Command.Resume), ProcessState.Active }
                    };
            }

            public ProcessState GetNext(Command command)
            {
                StateTransition transition = new StateTransition(CurrentState, command);
                ProcessState nextState;
                if (!transitions.TryGetValue(transition, out nextState))
                    throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
                return nextState;
            }

            public ProcessState MoveNext(Command command)
            {
                CurrentState = GetNext(command);
                return CurrentState;
            }
        }


        static void Main(string[] args)
        {
            Process p = new Process();
            Console.WriteLine("Current State = " + p.CurrentState);
            Console.WriteLine("Command.Begin: Current State = " + p.MoveNext(Command.Begin));
            Console.WriteLine("Command.Pause: Current State = " + p.MoveNext(Command.Pause));
            Console.WriteLine("Command.End: Current State = " + p.MoveNext(Command.End));
            Console.WriteLine("Command.Exit: Current State = " + p.MoveNext(Command.Exit));
            Console.ReadLine();
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
