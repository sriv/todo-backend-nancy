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
            this.After.AddItemToEndOfPipeline(x => x.Response.WithHeader("access-control-allow-origin", "*")
                .WithHeader("access-control-allow-headers", "Accept, Origin, Content-type"));

            Get["/"] = GetTodos;

            Get["/todos"] = GetTodos;

            Get["/todo/{id}"] = _ => GetTodo(_.id);

            Patch["/todo/{id}"] = UpdateTodo;

            Post["/"] = PostTodo;

            Options["/"] = _ => Negotiate.WithHeader("access-control-allow-methods", "GET,HEAD,POST,DELETE,OPTIONS,PUT");
            
            Options["/todo/{id}"] = _ => Negotiate.WithHeader("access-control-allow-methods", "GET,HEAD,DELETE,OPTIONS,PATCH");

            Delete["/"] = ClearTodos;

            Delete["/todo/{id}"] = ClearTodo;
        }

        private dynamic ClearTodos(dynamic parameters)
        {
            repo.Clear();
            return HttpStatusCode.OK;
        }

        private dynamic ClearTodo(dynamic parameters)
        {
            repo.Delete(parameters.id);
            return HttpStatusCode.OK;
        }

        private dynamic GetTodos(dynamic parameters)
        {
            return repo.All();
        }

        private dynamic GetTodo(int order)
        {
            return repo.Get(order);
        }

        private dynamic PostTodo(dynamic parameters)
        {
            var todo = repo.Add(this.Bind<Todo>(), Context.Request.Url.SiteBase);
            return Negotiate.WithModel(todo)
                .WithStatusCode(HttpStatusCode.Created);
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
                .WithStatusCode(HttpStatusCode.Created);
        }
    }
}