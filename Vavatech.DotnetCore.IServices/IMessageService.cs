using System;
using System.Collections.Generic;
using System.Text;

namespace Vavatech.DotnetCore.IServices
{
    public interface IMessageService
    {
        void Send(string message);
    }
}
