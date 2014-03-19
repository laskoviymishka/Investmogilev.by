// // -----------------------------------------------------------------------
// // <copyright file="IOpenUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IOpenUoW
	{
		void OnOpenExit();
		void OnOpenEntry();

		bool FromMapToOpen();
		bool FromOpenToMap();
	}
}