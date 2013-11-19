using Invest.Common.Model;
using System;
using System.Collections.Generic;
using System.Web.Http;
using MongoRepository;
using MongoRepository.Repository;

namespace AdminPanelUI.Controllers
{
    public class RegionApiController : ApiController
    {
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
        public IEnumerable<Region> Get()
        {
            return _repo.All<Region>();
        }

        // GET api/regionapi/5
        public Region Get(string id)
        {
            return _repo.GetById<Region>(r => r._id == id);
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
