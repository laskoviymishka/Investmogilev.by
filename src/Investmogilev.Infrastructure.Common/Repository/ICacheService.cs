// // -----------------------------------------------------------------------
// // <copyright file="ICacheService.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Repository
{
	#region Using

	using System;

	#endregion

	public interface ICacheService
	{
		T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
	}
}