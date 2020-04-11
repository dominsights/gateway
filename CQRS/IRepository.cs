using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : class;
        CommandResponse CreateFromRequest<T>(T item) where T : class;
        CommandResponse Update<T>(T item) where T : class;
    }
}
