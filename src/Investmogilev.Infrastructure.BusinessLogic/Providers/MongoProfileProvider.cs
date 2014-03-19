// // -----------------------------------------------------------------------
// // <copyright file="MongoProfileProvider.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Providers
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Configuration;
	using System.Linq;
	using System.Web.Hosting;
	using System.Web.Profile;
	using MongoDB.Bson;
	using MongoDB.Driver;
	using MongoDB.Driver.Builders;

	#endregion

	public class MongoProfileProvider : ProfileProvider
	{
		private MongoCollection _mongoCollection;

		public override string ApplicationName { get; set; }

		public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption,
			DateTime userInactiveSinceDate)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.LTE("LastActivityDate", userInactiveSinceDate));

			if (authenticationOption != ProfileAuthenticationOption.All)
			{
				query = Query.And(query, Query.EQ("IsAnonymous", authenticationOption == ProfileAuthenticationOption.Anonymous));
			}

			return (int) _mongoCollection.Remove(query).DocumentsAffected;
		}

		public override int DeleteProfiles(string[] usernames)
		{
			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName),
				Query.In("Username", new BsonArray(usernames)));
			return (int) _mongoCollection.Remove(query).DocumentsAffected;
		}

		public override int DeleteProfiles(ProfileInfoCollection profiles)
		{
			return DeleteProfiles(profiles.Cast<ProfileInfo>().Select(profile => profile.UserName).ToArray());
		}

		public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption,
			string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			return GetProfiles(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize,
				out totalRecords);
		}

		public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption,
			string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return GetProfiles(authenticationOption, usernameToMatch, null, pageIndex, pageSize, out totalRecords);
		}

		public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption,
			DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			return GetProfiles(authenticationOption, null, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
		}

		public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex,
			int pageSize, out int totalRecords)
		{
			return GetProfiles(authenticationOption, null, null, pageIndex, pageSize, out totalRecords);
		}

		public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption,
			DateTime userInactiveSinceDate)
		{
			IMongoQuery query = GetQuery(authenticationOption, null, userInactiveSinceDate);
			return (int) _mongoCollection.Count(query);
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
			SettingsPropertyCollection collection)
		{
			var settingsPropertyValueCollection = new SettingsPropertyValueCollection();

			if (context == null || collection == null || collection.Count < 1)
			{
				return settingsPropertyValueCollection;
			}

			var username = (string) context["_userName"];

			if (string.IsNullOrWhiteSpace(username))
			{
				return settingsPropertyValueCollection;
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Username", username));
			var bsonDocument = _mongoCollection.FindOneAs<BsonDocument>(query);

			foreach (SettingsProperty settingsProperty in collection)
			{
				var settingsPropertyValue = new SettingsPropertyValue(settingsProperty);
				settingsPropertyValueCollection.Add(settingsPropertyValue);

				object value = BsonTypeMapper.MapToDotNetValue(bsonDocument[settingsPropertyValue.Name]);

				if (value != null)
				{
					settingsPropertyValue.PropertyValue = value;
					settingsPropertyValue.IsDirty = false;
					settingsPropertyValue.Deserialized = true;
				}
			}

			UpdateBuilder update = Update.Set("LastActivityDate", DateTime.Now);
			_mongoCollection.Update(query, update);

			return settingsPropertyValueCollection;
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;

			_mongoCollection =
				new MongoClient(config["connectionString"] ?? "mongodb://localhost").GetServer()
					.GetDatabase(config["database"] ?? "ASPNETDB")
					.GetCollection(config["collection"] ?? "Profiles");
			_mongoCollection.EnsureIndex("ApplicationName");
			_mongoCollection.EnsureIndex("ApplicationName", "IsAnonymous");
			_mongoCollection.EnsureIndex("ApplicationName", "IsAnonymous", "LastActivityDate");
			_mongoCollection.EnsureIndex("ApplicationName", "IsAnonymous", "LastActivityDate", "Username");
			_mongoCollection.EnsureIndex("ApplicationName", "IsAnonymous", "Username");
			_mongoCollection.EnsureIndex("ApplicationName", "LastActivityDate");
			_mongoCollection.EnsureIndex("ApplicationName", "Username");
			_mongoCollection.EnsureIndex("ApplicationName", "Username", "IsAnonymous");

			base.Initialize(name, config);
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
		{
			var username = (string) context["_userName"];
			var isAuthenticated = (bool) context["IsAuthenticated"];

			if (string.IsNullOrWhiteSpace(username) || collection.Count < 1)
			{
				return;
			}

			var values = new Dictionary<string, object>();

			foreach (SettingsPropertyValue settingsPropertyValue in collection)
			{
				if (!settingsPropertyValue.IsDirty)
				{
					continue;
				}

				if (!isAuthenticated && !(bool) settingsPropertyValue.Property.Attributes["AllowAnonymous"])
				{
					continue;
				}

				values.Add(settingsPropertyValue.Name, settingsPropertyValue.PropertyValue);
			}

			IMongoQuery query = Query.And(Query.EQ("ApplicationName", ApplicationName), Query.EQ("Username", username));
			var bsonDocument = _mongoCollection.FindOneAs<BsonDocument>(query);

			if (bsonDocument == null)
			{
				bsonDocument = new BsonDocument
				{
					{"ApplicationName", ApplicationName},
					{"Username", username}
				};
			}

			var mergeDocument = new BsonDocument
			{
				{"LastActivityDate", DateTime.Now},
				{"LastUpdatedDate", DateTime.Now}
			};

			mergeDocument.AddRange(values as IDictionary<string, object>);
			bsonDocument.Merge(mergeDocument);

			_mongoCollection.Save(bsonDocument);
		}

		#region Private Methods

		private static ProfileInfo ToProfileInfo(BsonDocument bsonDocument)
		{
			return new ProfileInfo(bsonDocument["Username"].AsString, bsonDocument["IsAnonymous"].AsBoolean,
				bsonDocument["LastActivityDate"].ToUniversalTime(), bsonDocument["LastUpdatedDate"].ToUniversalTime(), 0);
		}

		private ProfileInfoCollection GetProfiles(ProfileAuthenticationOption authenticationOption, string usernameToMatch,
			DateTime? userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			IMongoQuery query = GetQuery(authenticationOption, usernameToMatch, userInactiveSinceDate);

			totalRecords = (int) _mongoCollection.Count(query);

			var profileInfoCollection = new ProfileInfoCollection();

			foreach (
				var bsonDocument in
					_mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex*pageSize).SetLimit(pageSize))
			{
				profileInfoCollection.Add(ToProfileInfo(bsonDocument));
			}

			return profileInfoCollection;
		}

		private IMongoQuery GetQuery(ProfileAuthenticationOption authenticationOption, string usernameToMatch,
			DateTime? userInactiveSinceDate)
		{
			IMongoQuery query = Query.EQ("ApplicationName", ApplicationName);

			if (authenticationOption != ProfileAuthenticationOption.All)
			{
				query = Query.And(query, Query.EQ("IsAnonymous", authenticationOption == ProfileAuthenticationOption.Anonymous));
			}

			if (!string.IsNullOrWhiteSpace(usernameToMatch))
			{
				query = Query.And(query, Query.Matches("Username", usernameToMatch));
			}

			if (userInactiveSinceDate.HasValue)
			{
				query = Query.And(query, Query.LTE("LastActivityDate", userInactiveSinceDate));
			}

			return query;
		}

		#endregion
	}
}