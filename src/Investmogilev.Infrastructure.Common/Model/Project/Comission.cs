﻿// // -----------------------------------------------------------------------
// // <copyright file="Comission.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.State;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	#endregion

	public class Comission : IMongoEntity
	{
		private List<string> _projectIdList;

		public AdditionalInfo Info { get; set; }

		public string Body { get; set; }

		public ComissionType Type { get; set; }

		public DateTime CommissionTime { get; set; }

		public List<string> ProjectIds
		{
			get
			{
				if (_projectIdList == null)
				{
					_projectIdList = new List<string>();
					foreach (
						var project in
							RepositoryContext.Current.All<Project>(p => p.WorkflowState.CurrentState == ProjectWorkflow.State.WaitComission))
					{
						_projectIdList.Add(project._id);
					}
				}

				return _projectIdList;
			}
			set { _projectIdList = value; }
		}

		[BsonIgnore]
		public List<Project> Projects
		{
			get { return RepositoryContext.Current.All<Project>(p => ProjectIds.Contains(p._id)).ToList(); }
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }
	}
}