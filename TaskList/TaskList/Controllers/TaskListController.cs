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

        // LogIn
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
                    TempData["uID"] = u.Id;
                    return RedirectToAction("TaskListView");
                }
            }
            ViewBag.logMsg = "Incorrect Email/Password";
            return View(Users);
        }

        // NewUser
        public IActionResult CreateUser()
        {
            ViewBag.logMsg = "Ready";
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User U)
        {
            if (ModelState.IsValid)
            {
                List<User> Users = _db.Users.ToList();
                foreach (User existingUser in Users)
                {
                    if (existingUser.Email == U.Email)
                    {
                        ViewBag.logMsg = $"User {U.Email} already in system. Please log in instead.";
                        return View("CreateUser", U);
                    }
                }
                _db.Users.Add(U);
                _db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            else
            {
                ViewBag.logMsg = "Invalid format.";
                return View("CreateUser", U);
            }
        }

        // TaskListView
        public IActionResult TaskListView(string sortOrder)
        {
            User U = _db.Users.Find(TempData["uID"]);
            ViewBag.CurrentUser = U;
            List<ToDoItem> ToDoList = _db.ToDoItems.ToList();
            List<ToDoItem> ToDoListUser = new List<ToDoItem>();
            foreach (ToDoItem t in ToDoList)
            {
                if (t.UserId == U.Id)
                {
                    ToDoListUser.Add(t);
                }
            }

            switch (sortOrder)
            {
                case "id":
                    ToDoListUser = ToDoListUser.OrderBy(x => x.Id).ToList();
                    break;
                case "deadline":
                    ToDoListUser = ToDoListUser.OrderBy(x => x.Deadline).ToList();
                    break;
                case "isdone":
                    ToDoListUser = ToDoListUser.OrderBy(x => x.IsDone).ToList();
                    break;
                default:
                    break;
            }

            TempData["uID"] = U.Id;
            return View(ToDoListUser);
        }

        public IActionResult ToggleTask(int Id)
        {
            ToDoItem t = _db.ToDoItems.Find(Id);
            User u = _db.Users.Find(t.UserId);
            t.IsDone = !t.IsDone;
            _db.ToDoItems.Update(t);
            _db.SaveChanges();
            TempData["uID"] = u.Id;
            return RedirectToAction("TaskListView");
        }

        public IActionResult DeleteTask(int Id)
        {
            ToDoItem t = _db.ToDoItems.Find(Id);
            User u = _db.Users.Find(t.UserId);
            _db.ToDoItems.Remove(t);
            _db.SaveChanges();
            TempData["uID"] = u.Id;
            return RedirectToAction("TaskListView");
        }

        // CreateTask
        public IActionResult CreateTask()
        {
            User U = _db.Users.Find(TempData["uID"]);
            ViewBag.CurrentUser = U;
            TempData["uID"] = U.Id;
            return View();
        }

        [HttpPost]
        public IActionResult CreateTask(ToDoItem T)
        {
            _db.ToDoItems.Add(T);
            _db.SaveChanges();
            User U = _db.Users.Find(TempData["uID"]);
            TempData["uID"] = U.Id;
            return RedirectToAction("TaskListView");
        }
    }
}
