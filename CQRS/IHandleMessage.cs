using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public interface IHandleMessage<in T>
    {
        Task HandleAsync(T message);
    }
}
