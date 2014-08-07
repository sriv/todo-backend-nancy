using System;
using Nancy;

namespace todo_backend_nancy
{
	public class TodoModule : NancyModule
	{
		public TodoModule()
		{
			this.After.AddItemToEndOfPipeline(x => x.Response.WithHeader("Access-Control-Allow-Origin", "*"));
			Get["/"] = _ => "foo";
		}
	}
}
