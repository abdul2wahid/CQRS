﻿using CQRS.Commands;
using CQRS.Repository;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.FrameWork
{
    public abstract class Aggregate
    {
        private readonly List<object> _uncommittedEvents = new List<object>();
        public object Id { get; protected set; }
        public long Version { get; private set; }

        protected Aggregate(object id)
        {
            Id = id;
        }

        protected Aggregate() { }

        public IEnumerable<object> GetUncommittedChanges()
        {
            return _uncommittedEvents;
        }

        internal void ClearUncommittedChanges()
        {
            _uncommittedEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                AggregateUpdater.Update(this, @event);
                Version++;
            }
        }

        protected void Apply(object @event)
        {
            _uncommittedEvents.Add(@event);
            AggregateUpdater.Update(this, @event);
            Version++;
        }

        private static class AggregateUpdater
        {
            private static readonly ConcurrentDictionary<Tuple<Type, Type>, Action<Aggregate, object>> Cache = new ConcurrentDictionary<Tuple<Type, Type>, Action<Aggregate, object>>();

            public static void Update(Aggregate instance, object @event)
            {
                var tuple = new Tuple<Type, Type>(instance.GetType(), @event.GetType());
                var action = Cache.GetOrAdd(tuple, ActionFactory);
                action(instance, @event);
            }

            private static Action<Aggregate, object> ActionFactory(Tuple<Type, Type> key)
            {
                var eventType = key.Item2;
                var aggregateType = key.Item1;

                const string methodName = "UpdateFrom";
                var method = aggregateType.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SingleOrDefault(x => x.Name == methodName && x.GetParameters().Single().ParameterType.IsAssignableFrom(eventType));

                if (method == null)
                    return (x, y) => { };

                return (instance, @event) => method.Invoke(instance, new[] { @event });
            }
        }
    }
}