using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Model
{
    public class EditParametrViewModel
    {
        public string RegionId { get; set; }
        public string ParametrName { get; set; }
        public List<KeyValuePair<int, double>> Values { get; set; }
    }
}
