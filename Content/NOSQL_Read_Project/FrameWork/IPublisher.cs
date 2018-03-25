using System;
using System.Collections.Generic;
using System.Text;

namespace NOSQL_Read_Project.FrameWork
{
    interface IPublisher
    {
        void Publish(IEnumerable<object> events);
    }
}
