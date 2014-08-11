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
            
            todo.Order = todos.Any() ? todos.Max(x => x.Order) + 1 : 1;
            todo.Completed = false;
            todo.Url = string.Format("{0}/todo/{1}", BasePath, todo.Order);

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

        public Todo Get(int order)
        {
            return odb.AsQueryable<Todo>().Where(x => x.Order == order).FirstOrDefault();
        }
    }
}