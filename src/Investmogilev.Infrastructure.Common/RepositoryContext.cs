﻿// // -----------------------------------------------------------------------
// // <copyright file="RepositoryContext.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common
{
	#region Using

	using System;
	using System.Collections;
	using System.Threading;
	using System.Web;
	using System.Web.Configuration;
	using Investmogilev.Infrastructure.Common.Repository;

	#endregion

	public static class RepositoryContext
	{
		private const string HTTPCONTEXTKEY = "Session.Base.HttpContext.Key";
		private static readonly Hashtable _threads = new Hashtable();

		/// <summary>
		///     Returns a database context or creates one if it doesn't exist.
		/// </summary>
		public static IRepository Current
		{
			get { return GetOrCreateSession(); }
		}

		/// <summary>
		///     Returns true if a database context is open.
		/// </summary>
		public static bool IsOpen
		{
			get
			{
				IRepository session = GetSession();
				return (session != null);
			}
		}

		#region Private Helpers

		private static IRepository GetOrCreateSession()
		{
			IRepository session = GetSession();
			if (session == null)
			{
				session = new MongoRepository(WebConfigurationManager.AppSettings["mongoServer"],
					WebConfigurationManager.AppSettings["mongoBase"]);

				SaveSession(session);
			}

			return session;
		}

		private static IRepository GetSession()
		{
			if (HttpContext.Current != null)
			{
				if (HttpContext.Current.Items.Contains(HTTPCONTEXTKEY))
				{
					return (IRepository) HttpContext.Current.Items[HTTPCONTEXTKEY];
				}

				return null;
			}
			Thread thread = Thread.CurrentThread;
			if (string.IsNullOrEmpty(thread.Name))
			{
				thread.Name = Guid.NewGuid().ToString();
				return null;
			}
			lock (_threads.SyncRoot)
			{
				return (IRepository) _threads[Thread.CurrentThread.Name];
			}
		}

		private static void SaveSession(IRepository session)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items[HTTPCONTEXTKEY] = session;
			}
			else
			{
				lock (_threads.SyncRoot)
				{
					_threads[Thread.CurrentThread.Name] = session;
				}
			}
		}

		#endregion
	}
}