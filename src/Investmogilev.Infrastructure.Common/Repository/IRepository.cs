// // -----------------------------------------------------------------------
// // <copyright file="IRepository.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Repository
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Investmogilev.Infrastructure.Common.Model.Common;

	#endregion

	public interface IRepository
	{
		void Delete<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity;
		void Delete<T>(T item) where T : IMongoEntity;
		void DeleteAll<T>() where T : IMongoEntity;
		T GetOne<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity;
		IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity;
		IQueryable<T> All<T>() where T : IMongoEntity;
		void Add<T>(T item) where T : IMongoEntity;
		void Add<T>(IEnumerable<T> items) where T : IMongoEntity;
		void Update<T>(T item) where T : IMongoEntity;
	}
}