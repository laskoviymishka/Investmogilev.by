using Investmogilev.Infrastructure.Common.State.StateAttributes;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces
{
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