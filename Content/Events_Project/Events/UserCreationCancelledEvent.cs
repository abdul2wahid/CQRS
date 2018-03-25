using System;
using System.Collections.Generic;
using System.Text;

namespace Events_Project.Events
{
    public class UserCreationCancelledEvent
    {

        public Guid UserId { get; private set; }
    

        public UserCreationCancelledEvent(Guid userId)
        {
            UserId = userId;
         
        }

    }
}
