using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.Abstractions;
using Validators.Polish;

namespace Vavatech.DotnetCore.WebApi.RouteConstraints
{

    // dotnet add package PolishValidators
    public class PeselRouteConstraint : IRouteConstraint
    {
        private readonly IValidator validator;

        public PeselRouteConstraint()
        {
            this.validator = new PeselValidator();
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object routeValue))
            {
                string number = routeValue.ToString();

                return validator.IsValid(number);
            }
            else
                return false;
        }
    }
}
