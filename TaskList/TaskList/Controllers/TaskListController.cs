using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TaskListController : Controller
    {
        private readonly TaskListDBContext _db;
        public TaskListController(TaskListDBContext db)
        {
            _db = db;
        }

        public IActionResult LogIn()
        {
            ViewBag.logMsg = "Ready";
            List<User> Users = _db.Users.ToList();
            return View(Users);
        }

        [HttpPost]
        public IActionResult LogIn(string Email, string Password)
        {
            bool UserAuth = false;
            List<User> Users = _db.Users.ToList();
            foreach (User u in Users)
            {
                if (Email.Equals(u.Email) && Password.Equals(u.Password))
                {
                    UserAuth = true;
                    ViewBag.UserId = u.Id;
                    ViewBag.logMsg = $"User ID{u.Id} found!";
                    return View();
                    //return RedirectToAction("TaskView", u.Id);
                }
            }
            ViewBag.logMsg = "Incorrect Email/Password";
            return View(Users);
        }
    }
}
