//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParametrValue
    {
        public ParametrValue()
        {
            this.CurrentDatas = new HashSet<CurrentData>();
            this.YearDatas = new HashSet<YearData>();
        }
    
        public System.Guid ID { get; set; }
        public string ParametrName { get; set; }
        public string RegionName { get; set; }
    
        public virtual ICollection<CurrentData> CurrentDatas { get; set; }
        public virtual Parametr Parametr { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<YearData> YearDatas { get; set; }
    }
}
