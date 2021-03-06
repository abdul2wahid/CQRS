﻿using CQRS.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Bus
{

        public class CommandBus :  ICommandService
        {
            readonly Dictionary<Type, List<Action<object>>> _handlers = new Dictionary<Type, List<Action<object>>>();

          

            public void RegisterHandler<T>(Action<T> handler)
            {
                List<Action<object>> handlers;

                if (!_handlers.TryGetValue(typeof(T), out handlers))
                {
                    handlers = new List<Action<object>>();
                    _handlers.Add(typeof(T), handlers);
                }

                handlers.Add(x => handler((T)x));
            }

            public void Handle(object command)
            {
                List<Action<object>> handlers;

                if (!_handlers.TryGetValue(command.GetType(), out handlers))
                    throw new InvalidOperationException(string.Format("No handler registered for command type {0}", command.GetType()));
                if (handlers.Count != 1) throw new InvalidOperationException("Cannot send to more than one handler");

                handlers[0](command);
            }
        }
    
}