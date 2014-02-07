using System;

namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class StateAttribute : Attribute
	{
		private readonly Type _stateType;
		private readonly string _state;

		public StateAttribute(string stateMachineName, string state)
		{
			StateMachineName = stateMachineName;
			_state = state;
		}

		public StateAttribute(Type stateType, string stateMachineName, string state)
		{
			_stateType = stateType;
			_state = state;
			StateMachineName = stateMachineName;
			_state = state;
		}

		public string StateMachineName { get; private set; }

		public object State
		{
			get
			{
				if (_stateType != null && _stateType.IsEnum)
				{
					return Enum.Parse(_stateType, _state);
				}

				return _state;
			}
		}
	}
}