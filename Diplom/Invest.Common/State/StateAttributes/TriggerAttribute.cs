﻿using System;

namespace Invest.Common.State.StateAttributes
{
    public class TriggerAttribute : Attribute
    {
        private string _from;
        private string _to;
        private string _triggerName;
        private Type _triggerType;
        private Type _stateType;

        public TriggerAttribute(string workflowName, string triggerName, string from, string to)
        {
            WorkflowName = workflowName;
            _triggerName = triggerName;
            _from = from;
            _to = to;
        }

        public TriggerAttribute(Type triggerType, Type stateType, string workflowName, string triggerName, string from, string to)
        {
            WorkflowName = workflowName;
            _triggerName = triggerName;
            _from = from;
            _to = to;
            _triggerType = triggerType;
            _stateType = stateType;

        }

        public string WorkflowName { get; private set; }

        public object TriggerName
        {
            get
            {
                if (_triggerType != null && _stateType != null && _stateType.IsEnum && _triggerType.IsEnum)
                {
                    return Enum.Parse(_triggerType, _triggerName);
                }
                return _triggerName;
            }
        }

        public object From
        {
            get
            {
                if (_triggerType != null && _stateType != null && _stateType.IsEnum && _triggerType.IsEnum)
                {
                    return Enum.Parse(_stateType, _from);
                }
                return _from;
            }
        }

        public object To
        {
            get
            {
                if (_triggerType != null && _stateType != null && _stateType.IsEnum && _triggerType.IsEnum)
                {
                    return Enum.Parse(_stateType, _to);
                }
                return _to;
            }
        }
    }
}