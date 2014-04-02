// // -----------------------------------------------------------------------
// // <copyright file="IOnComissionUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IOnComissionUoW
	{
		void OnOnComissionExit();

		void OnOnComissionEntry();

		bool CouldComissionFix();

		bool CouldToIspolcom();
	}
}