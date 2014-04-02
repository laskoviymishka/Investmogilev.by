// // -----------------------------------------------------------------------
// // <copyright file="MongoEntity.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public interface IMongoEntity
	{
		string _id { get; set; }
	}
}