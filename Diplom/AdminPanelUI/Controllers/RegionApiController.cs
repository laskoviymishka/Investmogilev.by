using MongoRepository.Model;
using MongoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdminPanelUI.Controllers
{
    public class RegionApiController : ApiController
    {
        #region Fields

        private readonly RegionRepository _repo;

        #endregion

        #region Constructor

        public RegionApiController()
        {
            _repo = new RegionRepository();
        }

        #endregion

        #region Api methdos

        // GET api/regionapi
        public IEnumerable<Region> Get()
        {
            return _repo.AllRegion();
        }

        // GET api/regionapi/5
        public Region Get(string id)
        {
            return _repo.GetRegionByID(id);
        }

        // POST api/regionapi
        public void Post(Region value)
        {
            _repo.InsertRegion(value);
        }

        // PUT api/regionapi/5
        public void Put(string id, Region value)
        {
            if (id == value._id.ToString())
            {
                _repo.UpdateOne<Region>(value);
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
