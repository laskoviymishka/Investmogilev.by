namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
    public interface IState
    {
        IStateContext Context { get; set; }
        void OnEntry();
        void OnExit();
    }
}
