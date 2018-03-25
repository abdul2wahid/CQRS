using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Commands;
using MongoDB.Bson;
using CQRS.FrameWork;
using Events_Project.Events;

namespace CQRS.Domain
{
    public class Users : Aggregate
    {
        public string  _name;
        public string _email;

        public Users(Guid id, string name, string email,string createdBy)
            : base(id)
        {
            Apply(new UserCreatedEvent(id, name, email));
        }

        public Users()
        {
        }

        public void Update(string name, string email)
        {
         
                Apply(new UserUpdatedEvent((Guid)Id, name,email));
        }

        private void UpdateFrom(UserCreatedEvent @event)
        {
            Id = @event.UserId;
            _name = @event.Name;
            _email = @event.Email;
        }

        private void UpdateFrom(UserUpdatedEvent @event)
        {
            Id = @event.UserId;
            _name = @event.Name;
            _email = @event.Email;
        }
    }

}