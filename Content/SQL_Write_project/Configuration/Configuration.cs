using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Bus;
using CQRS.Commands;
using CQRS.Domain;

using CQRS.FrameWork;

using CQRS.Repository;

namespace CQRS.Configurations
{
    public class Configuration
    {
        private readonly CommandBus _bus;
       

        public CommandBus Bus { get { return _bus; } }
      

        private static readonly Configuration Config = new Configuration();
        public static Configuration Instance()
        {
            return Config;
        }

        private Configuration()
        {
            _bus = new CommandBus();
            var eventStore = new SqlStore(_bus);
            var repository = new DomainRepository(eventStore);

            //Registering my Commands to corresponding CommandHandlers
            var commandService = new UserCommandHandler(repository);
            _bus.RegisterHandler<CreateUserCommand>(commandService.Handle);
            _bus.RegisterHandler<UpdateUserCommand>(commandService.Handle);

         

        }
    }
}