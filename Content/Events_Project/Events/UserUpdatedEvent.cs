using System;

namespace Events_Project.Events
{
    public class UserUpdatedEvent
    {
        public Guid UserId { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UserUpdatedEvent(Guid userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }

     
    }
}