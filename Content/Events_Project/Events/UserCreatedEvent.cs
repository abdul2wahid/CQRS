
using System;

namespace Events_Project.Events
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(Guid userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }

      
    }
}