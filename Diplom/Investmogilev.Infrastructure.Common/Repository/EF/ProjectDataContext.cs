using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Investmogilev.Infrastructure.Common.Model.Common;
using Investmogilev.Infrastructure.Common.Model.Project;
using Investmogilev.Infrastructure.Common.Model.User;

namespace Investmogilev.Infrastructure.Common.Repository.EF
{
	public class ProjectDataContext : DbContext
	{
		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectNotes> Notes { get; set; }
		public DbSet<TaskTemplate> TaskTemplates { get; set; }
		public DbSet<MailTemplate> MailTemplates { get; set; }
		public DbSet<MessageQueue> MessageQueues { get; set; }
		public DbSet<NotificationQueue> NotificationQueues { get; set; }
		public DbSet<Users> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Configure Code First to ignore PluralizingTableName convention 
			// If you keep this convention then the generated tables will have pluralized names. 
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Entity<Project>()
				.HasMany(p => p.ProjectNotes)
				.WithOptional(p => p.Project)
				.HasForeignKey(p => p.ProjectId);
			modelBuilder.Entity<Project>().HasMany(p => p.Tasks).WithOptional(p => p.Project).HasForeignKey(p => p.ProjectId);
			modelBuilder.Entity<Project>().HasMany(p => p.Responses).WithOptional(p => p.Project).HasForeignKey(p => p.ProjectId);
			modelBuilder.Entity<InvestorResponse>().HasKey(t => t.ResponseId);
			modelBuilder.Entity<ProjectTask>().HasMany(p => p.TaskReport).WithOptional(p => p.Task).HasForeignKey(p => p.TaskId);
			modelBuilder.Entity<Comission>().Ignore(c => c.ProjectIds);
			modelBuilder.Entity<Comission>()
				.HasMany(c => c.Projects)
				.WithOptional(c => c.ProjectComission)
				.HasForeignKey(c => c.ProjectComissionId);
			modelBuilder.Entity<Comission>()
				.HasMany(c => c.Projects)
				.WithOptional(c => c.ProjectIspolcom)
				.HasForeignKey(c => c.ProjectIspolcomId);
			modelBuilder.ComplexType<Address>();
			modelBuilder.ComplexType<MessageBody>();
			modelBuilder.ComplexType<UserProfile>();
			modelBuilder.Entity<Report>()
				.HasMany(p => p.ReportResponses)
				.WithOptional(p => p.Report)
				.HasForeignKey(p => p.ReportId);
			modelBuilder.Entity<Report>().Ignore(r => r.ReportResponse);

			modelBuilder.Entity<Users>()
				.HasMany(u => u.Messages)
				.WithOptional(m => m.FromUser)
				.HasForeignKey(p => p.FromUserId);

			modelBuilder.Entity<Users>()
				.HasMany(u => u.Messages)
				.WithOptional(m => m.ToUser)
				.HasForeignKey(p => p.ToUserId);

			modelBuilder.Entity<Users>()
				.HasMany(u => u.Notifications)
				.WithOptional(n => n.User)
				.HasForeignKey(n => n.UserId);

			modelBuilder.Entity<Users>()
				.HasMany(u => u.Projects)
				.WithOptional(n => n.Investor)
				.HasForeignKey(n => n.InvestorUser);

			modelBuilder.Entity<Users>().HasMany(u => u.Roles).WithMany(r => r.User);
		}

		public DbSet<T> GetDbSet<T>() where T : class, IMongoEntity
		{
			switch (typeof (T).Name)
			{
				case "Project":
				{
					return Projects as DbSet<T>;
				}
				case "ProjectNotes":
				{
					return Notes as DbSet<T>;
				}
				case "TaskTemplate":
				{
					return TaskTemplates as DbSet<T>;
				}
				case "MailTemplate":
				{
					return MailTemplates as DbSet<T>;
				}
				case "MessageQueue":
				{
					return MessageQueues as DbSet<T>;
				}
				case "NotificationQueue":
				{
					return NotificationQueues as DbSet<T>;
				}
				case "Users":
				{
					return Users as DbSet<T>;
				}
				case "Role":
				{
					return Roles as DbSet<T>;
				}
			}

			return null;
		}
	}
}