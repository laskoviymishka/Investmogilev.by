﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbRegionsEntities : DbContext
    {
        public dbRegionsEntities()
            : base("name=dbRegionsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AgregateParametr> AgregateParametrs { get; set; }
        public DbSet<CurrentData> CurrentDatas { get; set; }
        public DbSet<Parametr> Parametrs { get; set; }
        public DbSet<ParametrValue> ParametrValues { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<RegionRate> RegionRates { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<YearData> YearDatas { get; set; }
    }
}
