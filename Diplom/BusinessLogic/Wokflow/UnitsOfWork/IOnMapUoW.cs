namespace BusinessLogic.Wokflow.UnitsOfWork
{
    public interface IOnMapUoW
    {
        void OnMapExit();
        void OnMapEntry();

        bool FromOnComissionToOnMap();
        bool FromOnIspolcomToOnMap();
        bool FromOnMapToOnMap();
        bool FromWaitComissionFixesToOnMap();
        bool FromWaitIspolcomFixesToOnMap();
        bool FromOpenToOnMap();
        bool FromOnMapToOpen();
        bool FromOnMapToInvestorApprove();
    }
}