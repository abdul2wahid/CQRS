using NOSQL_Read_Project.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NOSQL_Read_Project.Bus
{
    public class EventBus:IPublisher
    {

        readonly Dictionary<Type, List<Action<object>>> _handlers = new Dictionary<Type, List<Action<object>>>();

        public void Publish(IEnumerable<object> events)
        {
            foreach (var @event in events)
                PublishEvent(@event);
        }

        public void PublishEvent(object @event)
        {
            var type = @event.GetType();
            var keys = _handlers.Keys.Where(x => x.IsAssignableFrom(type));

            foreach (var key in keys)
                foreach (var handler in _handlers[key])
                    handler(@event);
        }

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
    }
}
