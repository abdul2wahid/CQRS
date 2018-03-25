using CQRS.Commands;
using CQRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.FrameWork
{
    class DomainRepository:IRepository
    {
        readonly IStore _store;

        public DomainRepository(IStore store)
        {
            _store = store;
        }

        public T GetById<T>(object id) where T : Aggregate
        {
            var events = _store.LoadEvents(id);
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);
            aggregate.LoadFromHistory(events);

            return aggregate;
        }

        public void Save(ICommand cmd, Aggregate aggregate)
        {
            var newEvents = aggregate.GetUncommittedChanges().ToList();
    
            _store.StoreEvents(cmd, aggregate.Id, newEvents);
            aggregate.ClearUncommittedChanges();
        }
    }
}
