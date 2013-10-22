using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoRepository.Model
{
    public class AdditionalRegionInfo
    {
        public string InfoName
        {
            get;
            set;
        }

        public InfoType InfoType
        {
            get;
            set;
        }

        public string InfoValue
        {
            get;
            set;
        }
    }
}
