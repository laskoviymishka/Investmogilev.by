using System.Collections.Generic;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
    public class EditParametrViewModel
    {
        public string RegionId { get; set; }
        public string ParametrName { get; set; }
        public IEnumerable<KeyValuePair<int, double>> Values { get; set; }
    }
}
