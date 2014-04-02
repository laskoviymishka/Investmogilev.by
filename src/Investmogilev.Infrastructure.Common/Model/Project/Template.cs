// // -----------------------------------------------------------------------
// // <copyright file="Template.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Project
{
	#region Using

	using System.ComponentModel.DataAnnotations;
	using Investmogilev.Infrastructure.Common.Localization;

	#endregion

	public class Template : Project
	{
		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_Area_Площадь_проекта")]
		[Required(ErrorMessageResourceType = typeof (LocalizationResource),
			ErrorMessageResourceName = "GreenField_Area_Введите_площадь_проекта")]
		public double Area { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_CadastrValue_Кадастровая_стоимость_проекта")
		]
		public double EstimateInvoiceAmount { get; set; }

		[Display(ResourceType = typeof (LocalizationResource), Name = "GreenField_CadastrValue_Кадастровая_стоимость_проекта")
		]
		public double CadastrValue { get; set; }
	}
}