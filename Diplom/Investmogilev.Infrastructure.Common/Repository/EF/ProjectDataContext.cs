using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;

namespace Investmogilev.Infrastructure.Common.Repository.EF
{
	public class ProjectDataContext : DbContext
	{
		public DbSet<Project> Projects { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Configure Code First to ignore PluralizingTableName convention 
			// If you keep this convention then the generated tables will have pluralized names. 
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Entity<Project>().HasKey(t => t._id);
			modelBuilder.Entity<ProjectTask>().HasKey(t => t._id);
			modelBuilder.Entity<Report>().HasKey(t => t._id);
			modelBuilder.Entity<AdditionalInfo>().HasKey(t => t._id);
		}
	}
}
