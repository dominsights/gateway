using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public interface IRepository
    {
        T GetById<T>(int id) where T : class;
        Task<CommandResponse> CreateFromRequestAsync<T>(T item) where T : class;
        CommandResponse Update<T>(T item) where T : class;
    }
}
