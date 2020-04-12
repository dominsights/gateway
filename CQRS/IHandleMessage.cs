using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public interface IHandleMessage<in T>
    {
        void Handle(T message);
    }
}
