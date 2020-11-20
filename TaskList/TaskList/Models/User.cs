using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(50, ErrorMessage = "Email cannot be more than 50 characters long.")]
        public string Email { get; set; }
        //[StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters long.")]
        //[RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!#$%^&-+=()])(?=\\S+$).{8, 50}$", ErrorMessage = "Password does not meet requirements, please review and try again.")]
        [RegularExpression(@"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^\s])[a-zA-Z0-9]{8,50}$", ErrorMessage = "Password does not meet requirements, please review and try again.")]
        public string Password { get; set; }

        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
