namespace Invest.Common.State.StateAttributes
{
    public abstract class BaseState : IState
    {
        protected BaseState(IStateContext context)
        {
            Context = context;
        }

        public abstract void OnEntry();

        public abstract void OnExit();

        public IStateContext Context { get; set; }
    }
}