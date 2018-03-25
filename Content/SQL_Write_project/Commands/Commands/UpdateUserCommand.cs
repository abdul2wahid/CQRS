﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Commands
{
    public class UpdateUserCommand: ICommand
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ModifiedBy { get; set; }

      
    }
}