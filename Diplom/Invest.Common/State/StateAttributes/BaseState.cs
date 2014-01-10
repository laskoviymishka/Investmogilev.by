namespace Invest.Common.State.StateAttributes
{
    public abstract class BaseState : IState
    {
        protected IStateContext Context { get; private set; }

        protected BaseState(IStateContext context)
        {
            Context = context;
        }

        public abstract void OnEntry();

        public abstract void OnExit();
    }
}