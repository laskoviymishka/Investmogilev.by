namespace Investmogilev.Infrastructure.StateMachine
{
	public partial class StateMachine<TState, TTrigger>
	{
		internal class StateReference
		{
			public TState State { get; set; }
		}
	}
}