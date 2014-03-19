// // -----------------------------------------------------------------------
// // <copyright file="IDoneUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	public interface IDoneUoW
	{
		void OnDoneExit();

		void OnDoneEntry();
	}
}