using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.API;

namespace Vavatech.DotnetCore.GrpcService.Services
{
    public class MyTrackingService : Tracking.API.TrackingService.TrackingServiceBase
    {
        private readonly ILogger<MyTrackingService> logger;

        public MyTrackingService(ILogger<MyTrackingService> logger)
        {
            this.logger = logger;
        }

        public override Task<AddLocationResponse> AddLocation(AddLocationRequest request, ServerCallContext context)
        {
            logger.LogInformation($"{request.DeviceId} {request.Latitude}:{request.Longitude} {request.Speed} {request.Direction}");

            var response = new AddLocationResponse { IsConfirmed = true };

            return Task.FromResult(response);
        
        }
    }
}
