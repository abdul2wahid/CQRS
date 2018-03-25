using CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.FrameWork
{
    public interface IRepository
    {
        T GetById<T>(object id) where T : Aggregate;
        void Save(ICommand cmd, Aggregate aggregate);
    }
}
