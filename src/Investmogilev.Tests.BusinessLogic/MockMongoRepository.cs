﻿// // -----------------------------------------------------------------------
// // <copyright file="MockMongoRepository.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Tests.BusinessLogic
{
	#region Using

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Repository;

	#endregion

	public class MockMongoRepository : IRepository
	{
		#region Private Fields

		private readonly IList _db;

		#endregion

		#region Constructor

		public MockMongoRepository(IList data)
		{
			_db = data;
		}

		#endregion

		#region Delete

		public void Delete<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			IQueryable<T> items = All<T>().Where(expression);
			foreach (var item in items)
			{
				Delete(item);
			}
		}

		public void Delete<T>(T item) where T : IMongoEntity
		{
			(_db as IList<T>).Remove(item);
		}

		public void DeleteAll<T>() where T : IMongoEntity
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Selector

		public T GetOne<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			return All<T>().Where(expression).SingleOrDefault();
		}

		public IQueryable<T> All<T>() where T : IMongoEntity
		{
			return (_db as IList<T>).AsQueryable();
		}

		public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			return All<T>().Where(expression);
		}

		#endregion

		#region Inster

		public void Add<T>(T item) where T : IMongoEntity
		{
			(_db as IList<T>).Add(item);
		}

		public void Add<T>(IEnumerable<T> items) where T : IMongoEntity
		{
			foreach (var item in items)
			{
				Add(item);
			}
		}

		#endregion

		#region Update

		public void Update<T>(T item) where T : IMongoEntity
		{
			if (GetOne<T>(t => t._id == item._id) != null)
			{
				var elem = GetOne<T>(t => t._id == item._id);
				Delete(item);
				Add(item);
			}
		}

		#endregion
	}
}