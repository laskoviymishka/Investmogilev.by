namespace Investmogilev.Infrastructure.Common.State
{
	public interface IStateMachine<TS> where TS : class
	{
		TS GetState { get; }
		bool TryFireTrigger(string trigger);

		bool CanFire(string trigger);
	}
}