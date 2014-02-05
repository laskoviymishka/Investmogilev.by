using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Investmogilev.Infrastructure.StateMachine;

namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
	public class AttributeStateMachineBuilder : IStateMachineBuilder
	{
		private static ConcurrentDictionary<Type, IState> _states;
		private string _stateMachineName;
		private IStateContext _context;

		public static void InitializeStates(Dictionary<Type, IState> container)
		{
			_states = new ConcurrentDictionary<Type, IState>(container);
		}

		public StateMachine<TS, TT> BuilStateMachine<TS, TT>(string statemachineName, IStateContext context, TS inititalState)
		{
			_context = context;
			_stateMachineName = statemachineName;
			List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(StateAttribute)))).ToList();

			var machine = new StateMachine<TS, TT>(inititalState);
			var getStates = new List<IState>();
			var transitions = new List<Transition>();

			foreach (var type in types)
			{
				var state = Activator.CreateInstance(type, _context) as IState;
				getStates.Add(state);
				foreach (var method in type.GetMethods())
				{
					var attributes = method.GetCustomAttributes(typeof(TriggerAttribute), true) as TriggerAttribute[];
					if (attributes != null && attributes.Length > 0)
					{
						var attribute = attributes.First(a => a.WorkflowName == _stateMachineName);

						if (attribute != null && type.GetInterface("IState") != null)
						{
							transitions.Add(new Transition
							{
								From = (TS)attribute.From,
								To = (TS)attribute.To,
								Trigger = (TT)attribute.TriggerName,
								Guard = GetReturningFunc<bool>(state, method.Name)
							});
						}
					}
				}
			}

			TransitionComparer comparer = new TransitionComparer();
			var pureTransitions = transitions.Distinct(comparer);

			StateMachine<TS, TT>.StateConfiguration stateconfigure;
			foreach (var state in getStates)
			{
				StateAttribute attribute = null;
				var attrs = state.GetType().GetCustomAttributes(typeof(StateAttribute), false) as StateAttribute[];
				if (attrs != null && attrs.Length > 0)
				{
					attribute = attrs[0];
				}
				stateconfigure = machine.Configure((TS)attribute.State).OnEntry(state.OnEntry).OnExit(state.OnExit);
				

				var fromTransitions = transitions.Where(
						t => t.From.ToString() == attribute.State.ToString()).ToArray();

				var couples = new Dictionary<KeyValuePair<object, object>, HashSet<Transition>>();

				for (int i = 0; i < fromTransitions.Length; i++)
				{
					for (int j = 0; j < fromTransitions.Length; j++)
					{
						if (i != j && fromTransitions[i].To.ToString() == fromTransitions[j].To.ToString()
							&& fromTransitions[i].Trigger.ToString() == fromTransitions[j].Trigger.ToString())
						{
							var couple = new KeyValuePair<object, object>(fromTransitions[i].To, fromTransitions[i].Trigger);

							if (!couples.ContainsKey(couple))
							{
								couples.Add(couple, new HashSet<Transition>());
							}

							couples[couple].Add(fromTransitions[i]);
						}
					}
				}

				var stateTransitions =
					transitions.Where(
						t => t.From.ToString() == attribute.State.ToString() && t.To.ToString() != attribute.State.ToString()
						&& !couples.ContainsKey(new KeyValuePair<object, object>(t.To,t.Trigger)));


				foreach (var transition in stateTransitions)
				{
					stateconfigure.PermitIf((TT)transition.Trigger, (TS)transition.To, transition.Guard);
				}

				foreach (var transition in transitions.Where(t => t.From.ToString() == attribute.State.ToString() && t.To.ToString() == attribute.State.ToString()))
				{
					stateconfigure.PermitReentryIf((TT)transition.Trigger, transition.Guard);
				}

				foreach (var key in couples.Keys)
				{
					var complexGuard = new ComplexGuard();
					foreach (var transiotion in couples[key])
					{
						complexGuard.AddGuard(transiotion.Guard);
					}
					if (key.Key.ToString() != attribute.State.ToString())
					{
						stateconfigure.PermitIf((TT)key.Value, (TS)key.Key, complexGuard.CheckComplex);
					}
					else
					{
						stateconfigure.PermitReentryIf((TT)key.Value, complexGuard.CheckComplex);
					}
				}
			}

			return machine;
		}

		private static Func<T> GetReturningFunc<T>(object x, string methodName)
		{
			var methodInfo = x.GetType().GetMethod(methodName);

			if (methodInfo == null ||
				methodInfo.ReturnType != typeof(T) ||
				methodInfo.GetParameters().Length != 0)
			{
				throw new ArgumentException();
			}

			var xRef = Expression.Constant(x);
			var callRef = Expression.Call(xRef, methodInfo);
			var lambda = (Expression<Func<T>>)Expression.Lambda(callRef);

			return lambda.Compile();
		}

		class ComplexGuard
		{
			private readonly List<Func<bool>> _innerGuards = new List<Func<bool>>();

			public void AddGuard(Func<bool> guard)
			{
				_innerGuards.Add(guard);
			}

			public bool CheckComplex()
			{
				return _innerGuards.All(innerGuard => innerGuard());
			}
		}
	}
}