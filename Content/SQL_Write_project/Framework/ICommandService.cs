using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.FrameWork
{
    interface ICommandService
    {
        void Handle(object command);
    }
}
