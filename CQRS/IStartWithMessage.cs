using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public interface IStartWithMessage<in T> where T : Message
    {
        void Handle(T message);
    }
}
