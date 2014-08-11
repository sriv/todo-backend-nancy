using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;

namespace todo_backend_nancy
{
    public class TodoModule : NancyModule
    {
        private TodoRepository repo;

        public TodoModule(TodoRepository repo)
        {
            this.repo = repo;
            this.After.AddItemToEndOfPipeline(x => x.Response.WithHeader("Access-Control-Allow-Origin", "*")
                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
                .WithHeader("Access-Control-Allow-Methods", "GET,HEAD,POST,DELETE,OPTIONS,PUT,PATCH"));

            Get["/"] = _ => Response.AsRedirect("~/todos");

            Get["/todos"] = GetTodos;

            Get["/todo/{order}"] = _ => GetTodo(_.order);

            Patch["/todo/{order}"] = UpdateTodo;

            Post["/"] = PostTodo;

            Options["/"] = _ => _;

            Delete["/"] = ClearTodo;
        }

        private dynamic ClearTodo(dynamic arg)
        {
            repo.Clear();
            return HttpStatusCode.OK;
        }

        public dynamic GetTodos(dynamic parameters)
        {
            return repo.All();
        }

        public dynamic GetTodo(int order)
        {
            return repo.Get(order);
        }

        public dynamic PostTodo(dynamic parameters)
        {
            var todo = repo.Add(this.Bind<Todo>(), Context.Request.Url.SiteBase);
            return Negotiate.WithModel(todo)
                .WithStatusCode(HttpStatusCode.Created);
        }

        public dynamic UpdateTodo(dynamic parameters)
        {
            var todo = repo.Update(this.Bind<Todo>());
            return Negotiate.WithModel(todo)
                .WithStatusCode(HttpStatusCode.Created);
        }
    }
}