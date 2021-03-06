﻿// // -----------------------------------------------------------------------
// // <copyright file="Workflow.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.State;
	using MongoDB.Bson.Serialization.Attributes;

	#endregion

	[BsonIgnoreExtraElements]
	public class Workflow
	{
		[Display(Name = "История изменения")]
		public List<History> History { get; set; }

		[Display(Name = "Текущее состояние")]
		public ProjectWorkflow.State CurrentState { get; set; }

		public override string ToString()
		{
			return EnumDescription.GetEnumDescription(CurrentState);
		}
	}
}