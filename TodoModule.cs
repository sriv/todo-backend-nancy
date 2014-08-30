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

        private Tuple<string, string>[] CorsHeaders = { Tuple.Create("Access-Control-Allow-Origin", "*"), 
                                                        Tuple.Create("Access-Control-Allow-Headers", "Accept, Origin, Content-type"),
                                                        Tuple.Create("Access-Control-Allow-Methods", "GET,HEAD,POST,DELETE,OPTIONS,PUT,PATCH")};
        public TodoModule(TodoRepository repo)
        {
            this.repo = repo;

            Get["/"] = GetTodos;

            Get["/todos"] = GetTodos;

            Get["/todo/{id}"] = GetTodo;

            Patch["/todo/{id}"] = UpdateTodo;

            Post["/"] = PostTodo;

            Options["/"] = _ => Negotiate.WithHeaders(CorsHeaders);

            Options["/todos"] = _ => Negotiate.WithHeaders(CorsHeaders);

            Delete["/"] = ClearTodos;

            Delete["/todos"] = ClearTodos;

            Delete["/todo/{id}"] = ClearTodo;
        }

        private dynamic ClearTodos(dynamic parameters)
        {
            repo.Clear();
            return Negotiate.WithHeaders(CorsHeaders).WithStatusCode(HttpStatusCode.OK);
        }

        private dynamic ClearTodo(dynamic parameters)
        {
            repo.Delete(parameters.id);
            return Negotiate.WithHeaders(CorsHeaders).WithStatusCode(HttpStatusCode.OK);
        }

        private dynamic GetTodos(dynamic parameters)
        {
            return Negotiate.WithHeaders(CorsHeaders).WithModel(repo.All());
        }

        private dynamic GetTodo(dynamic parameters)
        {
            Todo todo = repo.Get(parameters.id);
            return Negotiate.WithHeaders(CorsHeaders).WithModel(todo);
        }

        private dynamic PostTodo(dynamic parameters)
        {
            var todo = repo.Add(this.Bind<Todo>(), string.Format("{0}://{1}", Context.Request.Url.Scheme, Context.Request.Url.HostName));
            return Negotiate.WithModel(todo)
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeaders(CorsHeaders);
        }

        private dynamic UpdateTodo(dynamic parameters)
        {
            var todo = repo.Get(parameters.id) as Todo;
            // TODO : need to figure out a good way of binding parameter updates
            var update = this.Bind<Todo>();
            if (!string.IsNullOrEmpty(update.Title))
                todo.Title = update.Title;
            if (update.Order.HasValue)
                todo.Order = update.Order.Value;
            if (update.Completed.HasValue)
                todo.Completed = update.Completed.Value;
            repo.Update(todo);
            return Negotiate.WithModel(repo.Get(parameters.id) as Todo)
                .WithStatusCode(HttpStatusCode.Created).WithHeaders(CorsHeaders);
        }
    }
}