﻿using System;

namespace Investmogilev.Infrastructure.StateMachine
{
    static class Enforce
    {
        public static T ArgumentNotNull<T>(T argument, string description)
            where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(description);

            return argument;
        }
    }
}