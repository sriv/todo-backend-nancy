using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NDatabase;
using NDatabase.Api.Query;

namespace todo_backend_nancy
{
    public class TodoRepository
    {

        private static NDatabase.Api.IOdb odb = OdbFactory.OpenInMemory();
        
        public Todo Add(Todo todo, string BasePath)
        {
            var todos = odb.QueryAndExecute<Todo>();
            var nextId = todos.Any() ? todos.Max(x => x.Id) + 1 : 1;

            var incompleteTodos = todos.Where<Todo>(x => !(x.Completed.HasValue && x.Completed.Value));
            var nextOrder = incompleteTodos.Any() ? incompleteTodos.Max(x => x.Order) + 1 : 1;
            todo.Order = todo.Order > 0 ? todo.Order : nextOrder;
            todo.Id = nextId;
            todo.Completed = false;
            todo.Url = string.Format("{0}/todo-backend/todo/{1}", BasePath, nextId);

            odb.Store<Todo>(todo);
            return todo;
        }

        public void Clear()
        {
            var todos = odb.QueryAndExecute<Todo>();
            foreach (var todo in todos)
                odb.Delete(todo);
        }

        public List<Todo> All()
        {
            return odb.QueryAndExecute<Todo>().ToList();
        }

        public Todo Get(int id)
        {
            return odb.AsQueryable<Todo>().Where(x => x.Id == id).FirstOrDefault();
        }

        public Todo Update(Todo todo)
        {
            odb.Store<Todo>(todo);
            return todo;
        }

        public void Delete(int id)
        {
            odb.Delete(Get(id));
        }
    }
}