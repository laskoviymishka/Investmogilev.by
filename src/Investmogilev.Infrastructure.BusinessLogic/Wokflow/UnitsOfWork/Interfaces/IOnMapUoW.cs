// // -----------------------------------------------------------------------
// // <copyright file="IOnMapUoW.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
	#region Using

	using Investmogilev.Infrastructure.Common.State.StateAttributes;

	#endregion

	public interface IOnMapUoW : IState
	{
		void OnMapExit();
		void OnMapEntry();

		bool FromOnComissionToOnMap();
		bool FromOnIspolcomToOnMap();
		bool FromOnMapToOnMap();
		bool FromWaitComissionFixesToOnMap();
		bool FromWaitIspolcomFixesToOnMap();
		bool FromOnMapToOpen();
		bool FromOnMapToInvestorApprove();
	}
}