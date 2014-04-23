// // -----------------------------------------------------------------------
// // <copyright file="StatisticsManager.cs"  company="One Call Care Management, Inc.">
// // Copyright (c) One Call Care Management, Inc. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------
namespace Investmogilev.Infrastructure.BusinessLogic.Managers
{
	using System;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Repository;
	using MongoDB.Bson;

	public class StatisticsManager
	{
		private readonly IRepository _repository;

		public StatisticsManager()
		{
			_repository = RepositoryContext.Current;
		}

		public void WriteActivity(string id, Activity activity)
		{
			var session = _repository.GetOne<StatSession>(s => s._id == id);
			if (session == null)
			{
				throw new NullReferenceException("there is no such session in DB");
			}

			if (session.Activities == null)
			{
				session.Activities = new List<Activity>();
			}

			session.Activities.Add(activity);
			_repository.Update(session);
		}

		public StatSession StartSession()
		{
			StatSession session = new StatSession
			{
				Time = DateTime.Now,
				Activities = new List<Activity>(),
				_id = ObjectId.GenerateNewId().ToString()
			};
			_repository.Add(session);
			return session;
		}

		public IEnumerable<StatSession> GetAllSessions(DateTime from, DateTime till)
		{
			return _repository.All<StatSession>(s => s.Time > from && s.Time < till);
		}
	}
}