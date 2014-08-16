using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace todo_backend_nancy
{
    public class Todo
    {
        // wont need nullable types if we can find a better way to bind the form values.
        public int Id { get; set; }
        public int? Order { get; set; }
        public string Title { get; set; }
        public string Url {get; set;}
        public bool? Completed { get; set; }
    }
}