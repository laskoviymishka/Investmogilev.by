//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegionRate
    {
        public System.Guid ID { get; set; }
        public string RateName { get; set; }
        public double RateValue { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
    
        public virtual Rate Rate { get; set; }
        public virtual Region Region1 { get; set; }
    }
}