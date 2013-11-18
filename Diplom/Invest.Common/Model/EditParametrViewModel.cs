using System.Collections.Generic;

namespace Invest.Common.Model
{
    public class EditParametrViewModel
    {
        public string RegionId { get; set; }
        public string ParametrName { get; set; }
        public IEnumerable<KeyValuePair<int, double>> Values { get; set; }
    }
}
