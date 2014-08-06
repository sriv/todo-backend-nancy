using System;
using Nancy.Hosting.Self;

namespace todo_backend_nancy
{
	class Program
	{
		public static void Main(string[] args)
		{
            var hostname = Environment.GetEnvironmentVariable("HOST");

            var port = Environment.GetEnvironmentVariable("PORT");
			var hostConfiguration = new HostConfiguration();
			hostConfiguration.RewriteLocalhost=false;
            var nancyHost = new NancyHost(hostConfiguration, new Uri("http://" + hostname + ":" + port));
            nancyHost.Start();
            Console.WriteLine("Listening at http://{0}:{1}", hostname, port);
            Console.ReadLine();
		}
	}
}