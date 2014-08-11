using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Conventions;

namespace todo_backend_nancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            
            this.Conventions.AcceptHeaderCoercionConventions.Add((acceptHeaders, ctx) =>
            {
                // Only json for Conn-neg
                return new [] { Tuple.Create("application/json", (decimal)1)};
            });
        }
    }
}