using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Vavatech.DotnetCore.IServices;

namespace Vavatech.DotnetCore.FakeServices
{
    public class FakeSmsMessageService : IMessageService
    {
        private readonly ILogger<FakeSmsMessageService> logger;

        public FakeSmsMessageService(ILogger<FakeSmsMessageService> logger)
        {
            this.logger = logger;
        }

        public void Send(string message)
        {
            logger.LogInformation($"Send sms {message}");
        }
    }
}
