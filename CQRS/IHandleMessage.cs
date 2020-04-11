using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    interface IHandleMessage<in T>
    {
        void Handle(T message);
    }
}
