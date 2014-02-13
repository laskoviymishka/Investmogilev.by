using System.Collections.Generic;
using System.Linq;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Repository.EF
{
	public class ProjectRepository : IRepository
	{
		private readonly ProjectDataContext _context;

		public ProjectRepository(ProjectDataContext context)
		{
			_context = context;
		}

		public void Delete<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> expression) where T : class, IMongoEntity
		{
			Delete(GetOne(expression));
		}

		public void Delete<T>(T item) where T : class,IMongoEntity
		{
			_context.GetDbSet<T>().Remove(item);
		}

		public void DeleteAll<T>() where T : class,IMongoEntity
		{
			throw new System.NotImplementedException();
		}

		public T GetOne<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> expression) where T : class, IMongoEntity
		{
			return All<T>().FirstOrDefault(expression);
		}

		public IQueryable<T> All<T>(System.Linq.Expressions.Expression<System.Func<T, bool>> expression) where T : class,IMongoEntity
		{
			return All<T>().Where(expression);
		}

		public IQueryable<T> All<T>() where T : class, IMongoEntity
		{
			return _context.GetDbSet<T>();
		}

		public void Add<T>(T item) where T : class, IMongoEntity
		{
			_context.GetDbSet<T>().Add(item);
			_context.SaveChanges();
		}

		public void Add<T>(IEnumerable<T> items) where T : class,IMongoEntity
		{
			_context.GetDbSet<T>().AddRange(items);
			_context.SaveChanges();
		}

		public void Update<T>(T item) where T :class, IMongoEntity
		{
			_context.SaveChanges();
		}
	}
}
