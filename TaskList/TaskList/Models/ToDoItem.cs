using System;
using System.Collections.Generic;

#nullable disable

namespace TaskList.Models
{
    public partial class ToDoItem
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsDone { get; set; }

        public virtual User User { get; set; }
    }
}
