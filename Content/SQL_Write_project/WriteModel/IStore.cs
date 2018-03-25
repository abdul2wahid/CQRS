using CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Repository
{
    public interface IStore
    {
        void StoreEvents(ICommand cm, object streamId, IEnumerable<object> events);
        IEnumerable<object> LoadEvents(object id, long version = 0);
    }
}
