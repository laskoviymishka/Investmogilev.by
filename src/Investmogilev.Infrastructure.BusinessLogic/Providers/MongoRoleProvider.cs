// // -----------------------------------------------------------------------
// // <copyright file="MongoRoleProvider.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Providers
{
	#region Using

	using System.Collections.Specialized;
	using System.Configuration.Provider;
	using System.Linq;
	using System.Web.Hosting;
	using System.Web.Security;
using Investmogilev.Infrastructure.Common;
using Investmogilev.Infrastructure.Common.Repository;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	#endregion

	public class MongoRoleProvider : RoleProvider
	{
		private MongoCollection _rolesMongoCollection;
		private MongoCollection _usersInRolesMongoCollection;
		private readonly IRepository _repository;

		public MongoRoleProvider()
		{
			_repository = RepositoryContext.Current;
		}

		public override string ApplicationName { get; set; }

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			// Make sure each role exists
			foreach (var roleName in roleNames)
			{
				if (!RoleExists(roleName))
				{
					throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
				}
			}

			foreach (var username in usernames)
			{
				MembershipUser membershipUser = Membership.GetUser(username);

				if (membershipUser == null)
				{
					throw new ProviderException(string.Format("The user '{0}' was not found.", username));
				}

				foreach (var roleName in roleNames)
				{
					if (IsUserInRole(username, roleName))
					{
						throw new ProviderException(string.Format("The user '{0}' is already in role '{1}'.", username, roleName));
					}

					var bsonDocument = new BsonDocument
					{
						{"ApplicationName", ApplicationName},
						{"Role", roleName},
						{"Username", username}
					};

					_usersInRolesMongoCollection.Insert(bsonDocument);
				}
			}
		}

		public override void CreateRole(string roleName)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName));

			if (_rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
			{
				throw new ProviderException(string.Format("The role '{0}' already exists.", roleName));
			}

			var bsonDocument = new BsonDocument
			{
				{"ApplicationName", ApplicationName},
				{"Role", roleName}
			};

			_rolesMongoCollection.Insert(bsonDocument);
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			if (!RoleExists(roleName))
			{
				throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName));

			if (throwOnPopulatedRole && _usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0)
			{
				throw new ProviderException("This role cannot be deleted because there are users present in it.");
			}

			_usersInRolesMongoCollection.Remove(query);
			_rolesMongoCollection.Remove(query);

			return true;
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			if (!RoleExists(roleName))
			{
				throw new ProviderException(string.Format("The role '{0}' was not found.", roleName));
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName));
			return
				_usersInRolesMongoCollection.FindAs<BsonDocument>(query)
					.ToList()
					.Select(bsonDocument => bsonDocument["Username"].AsString)
					.ToArray();
		}

		public override string[] GetAllRoles()
		{
			IMongoQuery query = Query.EQ("ApplicationName", ApplicationName);
			return
				_rolesMongoCollection.FindAs<BsonDocument>(query)
					.ToList()
					.Select(bsonDocument => bsonDocument["Role"].AsString)
					.ToArray();
		}

		public override string[] GetRolesForUser(string username)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Username", username));
			return
				_usersInRolesMongoCollection.FindAs<BsonDocument>(query)
					.ToList()
					.Select(bsonDocument => bsonDocument["Role"].AsString)
					.ToArray();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName));
			return
				_usersInRolesMongoCollection.FindAs<BsonDocument>(query)
					.ToList()
					.Select(bsonDocument => bsonDocument["Username"].AsString)
					.ToArray();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;

			MongoDatabase mongoDatabase =
				new MongoClient(config["connectionString"] ?? "mongodb://localhost").GetServer()
					.GetDatabase(config["database"] ?? "ASPNETDB");
			_rolesMongoCollection = mongoDatabase.GetCollection(config["collection"] ?? "Roles");
			_usersInRolesMongoCollection = mongoDatabase.GetCollection("UsersInRoles");

			_rolesMongoCollection.EnsureIndex("ApplicationName");
			_rolesMongoCollection.EnsureIndex("ApplicationName", "Role");
			_usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role");
			_usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Username");
			_usersInRolesMongoCollection.EnsureIndex("ApplicationName", "Role", "Username");

			base.Initialize(name, config);
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName),
				Query.EQ("Username", username));
			return _usersInRolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			foreach (var username in usernames)
			{
				foreach (var roleName in roleNames)
				{
					IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName),
						Query.EQ("Username", username));
					_usersInRolesMongoCollection.Remove(query);
				}
			}
		}

		public override bool RoleExists(string roleName)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Role", roleName));
			return _rolesMongoCollection.FindAs<BsonDocument>(query).Count() > 0;
		}
	}
}