using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public interface IStartWithMessage<in T> where T : Message
    {
        Task HandleAsync(T message);
    }
}
