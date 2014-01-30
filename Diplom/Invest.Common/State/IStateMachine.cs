namespace Invest.Common.State
{
	public interface IStateMachine<TS> where TS : class
	{
		bool TryFireTrigger(string trigger);

		bool CanFire(string trigger);

		TS GetState { get; }
	}
}