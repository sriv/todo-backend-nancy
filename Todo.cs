using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace todo_backend_nancy
{
    public class Todo
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public string Url {get; set;}
        public bool Completed { get; set; }
    }
}