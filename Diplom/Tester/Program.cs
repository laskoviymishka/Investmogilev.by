using DataAccess.Model;
using Excel;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MongoRepository.Model;
using MongoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static void WriteGreenField()
        {

            FileStream stream = File.Open(@"d:/Перечень земучастков для заключения ИД 01.10.2013+участки вне перечня.xls", FileMode.Open, FileAccess.Read);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet();

            ProjectRepository repo = new ProjectRepository();
            var table = result.Tables[0];
            for (int i = 6; i <= table.Rows.Count; i++)
            {
                try
                {
                    GreenField gr = new GreenField();
                    gr.AddressName = table.Rows[i].Field<string>(3);
                    gr.Appendix = table.Rows[i].Field<string>(11);
                    gr.Area = table.Rows[i].Field<double>(5);
                    gr.CadastrValue = table.Rows[i].Field<double>(6);
                    gr.Contact = table.Rows[i].Field<string>(9);
                    gr.Description = table.Rows[i].Field<string>(4);
                    gr.EstimateInvestDate = DateTime.FromOADate(table.Rows[i].Field<double>(2));
                    gr.Goal = table.Rows[i].Field<string>(4);
                    gr.Infrastructure = table.Rows[i].Field<string>(8);
                    gr.InvestNumber = table.Rows[i].Field<string>(1);
                    if (table.Rows[i].ItemArray[10].ToString() != "")
                    {
                        gr.InvestRequest = (int)table.Rows[i].Field<double>(10);
                    }
                    else
                    {
                        gr.InvestRequest = 0;
                    }
                    gr.Name = table.Rows[i].Field<string>(1);
                    gr.Owner = table.Rows[i].Field<string>(9);
                    gr.OwnerCity = "Могилев";
                    gr.Region = "Могилев";
                    gr.ThirdPartiePretender = table.Rows[i].Field<string>(7);
                    gr.Address = new Address() { Lat = 1, Lng = 1 };
                    Console.WriteLine("insert");
                    repo.InsertProject(gr);
                }
                catch
                {
                    Console.WriteLine("fail");
                }
            }

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
        }

        private static void WriteBrownField()
        {
            FileStream stream = File.Open(@"d:/Перечень обращений по инвестдоговорам 2013.xls", FileMode.Open, FileAccess.Read);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet();

            ProjectRepository repo = new ProjectRepository();
            var table = result.Tables["Принятые решения ОИК "];
            for (int i = 8; i <= table.Rows.Count; i++)
            {
                try
                {
                    BrownField br = new BrownField();
                    br.Contact = table.Rows[i].Field<string>(1);
                    br.Description = table.Rows[i].Field<string>(2);
                    br.Name = table.Rows[i].Field<string>(8);
                    br.EstimateRelease = DateTime.FromOADate(table.Rows[i].Field<double>(11));
                    br.InvestAmount = table.Rows[i].Field<double>(9);
                    br.RegistrationDate = DateTime.FromOADate(table.Rows[i].Field<double>(3));
                    br.Region = "";
                    br.WorkPlaces = (int)table.Rows[i].Field<double>(10);
                    br.AddressName = table.Rows[i].Field<string>(2);
                    br.Address = new Address() { Lat = 1, Lng = 1 };
                    Console.WriteLine("asd");
                    repo.InsertProject(br);
                }
                catch { }
            }

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
        }
    }
}
