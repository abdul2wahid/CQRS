using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Commands;
using CQRS.FrameWork;

namespace CQRS.Domain
{
    public class UserCommandHandler
    {
        private readonly IRepository _repository;

        public UserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateUserCommand command)
        {
            //Do your Domain logic here. Validation sholud not be done here ,It should have been done before raising the command
            var account = new Users(command.UserId, command.Name,command.Email, command.CreatedBy);
            _repository.Save(command,account);
        }

        public void Handle(UpdateUserCommand command)
        {
            var account = _repository.GetById<Users>(command.UserId);
            account.Update(command.Name,command.Email);
            _repository.Save(command,account);
        }

    }
}