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
            List<User> Users = _db.Users.ToList();
            foreach (User u in Users)
            {
                if (Email.Equals(u.Email) && Password.Equals(u.Password))
                {
                    ViewBag.UserId = u.Id;
                    ViewBag.logMsg = $"User ID{u.Id} found!";
                    return View();
                    //return RedirectToAction("TaskListView", u);
                }
            }
            ViewBag.logMsg = "Incorrect Email/Password";
            return View(Users);
        }

        public IActionResult NewUser()
        {
            ViewBag.logMsg = "Ready";
            return View();
        }

        [HttpPost]
        public IActionResult NewUser(User U)
        {
            if (ModelState.IsValid)
            {
                List<User> Users = _db.Users.ToList();
                foreach (User existingUser in Users)
                {
                    if (existingUser.Email == U.Email)
                    {
                        ViewBag.logMsg = $"User {U.Email} already in system. Please log in instead.";
                        return View("NewUser", U);
                    }
                }
                _db.Users.Add(U);
                _db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            else
            {
                ViewBag.logMsg = "Invalid format.";
                return View("NewUser", U);
            }
        }

        public IActionResult TaskListView(User U)
        {
            ViewBag.logMsg = "Ready";
            return View();
        }
    }
}
