// // -----------------------------------------------------------------------
// // <copyright file="IMinEconomyUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IMinEconomyUoW
	{
		void OnInMinEconomyExit();

		void OnInMinEconomyEntry();

		bool CouldMinEconomyResponsed();
	}
}