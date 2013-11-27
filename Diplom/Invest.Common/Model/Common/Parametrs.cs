using System.Collections.Generic;

namespace Invest.Common.Model.Common
{
    public class Parametrs
    {
        public string ParametrName
        {
            get;
            set;
        }

        public double IntegralValue
        {
            get;
            set;
        }

        public List<KeyValuePair<int,double>> Values
        {
            get;
            set;
        }

        public List<KeyValuePair<int, double>> AbsoluteValues
        {
            get;
            set;
        }

        public List<KeyValuePair<int, double>> DependValues
        {
            get;
            set;
        }

        public List<KeyValuePair<int, double>> DependAbsoluteValues
        {
            get;
            set;
        }

        public IList<Parametrs> ChildParametrs
        {
            get;
            set;
        }
    }
}
