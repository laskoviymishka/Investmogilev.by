using System.Web.Http.ModelBinding;
using Invest.Common.Model;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Invest.Common.Repository;
using MongoRepository;

namespace AdminPanelUI.Controllers
{
    public class RegionViewModel
    {
        public string RegionName { get; set; }
        public List<ParametrViewModel> ParametrViewModel { get; set; }
    }

    public class ParametrViewModel
    {
        public string ParentParametrName { get; set; }
        public string ParametrName { get; set; }
        public List<KeyValuePair<int, double>> Values { get; set; }
        public double Integral { get; set; }
    }
    public class RegionApiController : ApiController
    {
        #region NestedTypes


        #endregion

        #region Fields

        private IRepository _repo;

        #endregion

        #region Constructor

        public RegionApiController()
        {
            _repo = RepositoryContext.Current;
        }

        #endregion

        #region Api methdos

        // GET api/regionapi
        public IEnumerable<RegionViewModel> Get()
        {
            List<RegionViewModel> model = new List<RegionViewModel>();
            foreach (Region region in _repo.All<Region>())
            {
                RegionViewModel rvm = new RegionViewModel();

                rvm.RegionName = region.RegionName;
                rvm.ParametrViewModel = new List<ParametrViewModel>();
                foreach (Parametrs parentParametrs in region.Parametrs)
                {
                    foreach (Parametrs child in parentParametrs.ChildParametrs)
                    {
                        ParametrViewModel prvm = new ParametrViewModel();
                        prvm.ParametrName = child.ParametrName;
                        prvm.ParentParametrName = parentParametrs.ParametrName;
                        prvm.Integral = child.IntegralValue;
                        prvm.Values = child.Values;
                        rvm.ParametrViewModel.Add(prvm);
                    }
                }
                model.Add(rvm);
            }
            return model;
        }

        // GET api/regionapi/5
        public Region Get(string id)
        {
            return RepositoryContext.Current.GetOne<Region>(r => r._id == id);
        }

        // POST api/regionapi
        public void Post(Region value)
        {
            _repo.Add(value);
        }

        // PUT api/regionapi/5
        public void Put(string id, Region value)
        {
            if (id == value._id.ToString())
            {
                _repo.Update(value);
            }
            else
            {
                throw new Exception();
            }
        }

        // DELETE api/regionapi/5
        public void Delete(string id)
        {
        }

        #endregion
    }
}
