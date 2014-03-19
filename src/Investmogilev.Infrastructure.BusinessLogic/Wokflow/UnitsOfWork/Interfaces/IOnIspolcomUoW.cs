// // -----------------------------------------------------------------------
// // <copyright file="IOnIspolcomUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IOnIspolcomUoW
	{
		void OnOnIspolcomExit();

		void OnOnIspolcomEntry();

		bool CouldToMinEconomy();

		bool CouldToIspolcomFix();
	}
}