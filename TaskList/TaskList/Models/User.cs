using System;
using System.Collections.Generic;

#nullable disable

namespace TaskList.Models
{
    public partial class User
    {
        public User()
        {
            ToDoItems = new HashSet<ToDoItem>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
