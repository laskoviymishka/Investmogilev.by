// // -----------------------------------------------------------------------
// // <copyright file="ProjectDataContext.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Repository.EF
{
	#region Using

	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.Project;

	#endregion

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