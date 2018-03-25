using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NOSQL_Read_Project.Bus;
using NOSQL_Read_Project.ReadModel;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Bson;
using NOSQL_Read_Project.MongoConn;
using Events_Project.Events;

namespace NOSQL_Read_Project.Events
{
    public class UsersEventHandler
    {
        public List<UsersReadModel> AccountInfos = new List<UsersReadModel>();
        readonly EventBus _bus;
        public void Handle(UserCreatedEvent @event)
        {
            try
            {


                MongoConnection mObj = MongoConnection.MongoConnectionInstance;
                mObj.Insertone(new UsersReadModel(@event.UserId,
                     @event.Name, @event.Email));
            }
            catch(Exception ex)
            {
                _bus.Publish((IEnumerable<object>)new UserCreationCancelledEvent(@event.UserId));
            }
        }

        public void Handle(UserUpdatedEvent @event)
        {
            var account = AccountInfos.FirstOrDefault(x => x.Id == @event.UserId);

            if (account != null)
            {

            }
               
        }

      
    }
}