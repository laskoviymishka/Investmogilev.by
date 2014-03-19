// // -----------------------------------------------------------------------
// // <copyright file="MessageBody.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.User
{
	#region Using

	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Investmogilev.Infrastructure.Common.Model.Common;

	#endregion

	public class MessageBody
	{
		[Required]
		[Display(Name = "Сообщение")]
		public string Text { get; set; }

		public List<AdditionalInfo> Appendix { get; set; }
	}
}