// // -----------------------------------------------------------------------
// // <copyright file="IInvestorApproveUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IInvestorApproveUoW
	{
		void OnInvestorApproveExit();
		void OnInvestorApproveEntry();

		bool FromInvestorApproveToDocument();
		bool FromInvestorApproveToInvestorResponsed();
		bool FromOnMapToInvestorApprove();
	}
}