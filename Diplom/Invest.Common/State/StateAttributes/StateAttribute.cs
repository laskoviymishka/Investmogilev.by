using System;

namespace Invest.Common.State.StateAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StateAttribute : Attribute
    {
        public StateAttribute(Type classType, string stateMachineName, string state)
        {
            ClassType = classType;
            StateMachineName = stateMachineName;
            State = state;
            if (ClassType.GetInterface("IState") == null)
            {
                throw new Exception();
            }
        }

        public Type ClassType { get; private set; }

        public string StateMachineName { get; private set; }

        public string State { get; private set; }
    }
}