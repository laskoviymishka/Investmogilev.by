using AdminPanelUI.Models;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdminPanelUI.Controllers
{
    public class ParametrRegionController : ApiController
    {
        #region Fields

        private readonly dbRegionsEntities db;

        #endregion

        #region Constructor

        public ParametrRegionController()
        {
            db = new dbRegionsEntities();
        }

        #endregion

        #region Api methods

        // GET api/ParametrRegionController
        public IEnumerable<Parametr> GetParametrs()
        {
            return db.Parametrs.ToArray();
        }

        // GET api/ParametrRegionController/5
        public IEnumerable<ParametrValue> GetParamsForRegion(string name)
        {
            return db.ParametrValues.Where(p => p.RegionName == name).ToArray();
        }

        // GET api/ParametrRegionController/5
        public IEnumerable<ParametrValue> Get(string id)
        {
            var model = db.ParametrValues.Where(p => p.RegionName == id).ToArray();
            return model;
        }

        // POST api/ParametrRegionController
        public void PostParametrValue([FromBody]RegionParametr value)
        {
            var oldValues = db.ParametrValues.Where(t => (t.ParametrName == value.ParametrName && t.RegionName == value.RegionName));
            foreach (var item in oldValues)
            {
                db.ParametrValues.Remove(item);
            }
            db.SaveChanges();
            ParametrValue par = new ParametrValue();
            par.ID = Guid.NewGuid();
            par.RegionName = value.RegionName;
            par.ParametrName = value.ParametrName;
            par.Parametr = db.Parametrs.Where(p => p.Name == value.ParametrName.Trim()).First();
            par.Region = db.Regions.Where(p => p.Name == value.RegionName.Trim()).First();
            par.CurrentDatas = new List<CurrentData>();
            par.CurrentDatas.Add(new CurrentData()
                {
                    ID = Guid.NewGuid(),
                    ParametrValue1 = par,
                    ParametrValue = par.ID,
                    CurrentValue = value.CurrentData.Current,
                    PreviosValue = value.CurrentData.Previos
                });

            if (value.YearDatas.Count() > 0)
            {
                par.YearDatas = new List<YearData>();
                foreach (var item in value.YearDatas)
                {
                    par.YearDatas.Add(new YearData()
                    {
                        ID = Guid.NewGuid(),
                        ParametrValue1 = par,
                        ParametrValue = par.ID,
                        Year = item.Year,
                        Value = item.Value
                    });
                }
            }

            db.ParametrValues.Add(par);
            db.SaveChanges();
        }

        // PUT api/ParametrRegionController/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ParametrRegionController/5
        public void Delete(int id)
        {
        }

        #endregion
    }
}
