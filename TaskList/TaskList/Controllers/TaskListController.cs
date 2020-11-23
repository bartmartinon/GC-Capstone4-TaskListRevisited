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
        // Set up the Log In page.
        public IActionResult LogIn()
        {
            ViewBag.logMsg = "Ready";
            List<User> Users = _db.Users.ToList();
            return View(Users);
        }

        // Check to see if the given values for the form match an existing User's.
        // If so, move on to the User's appropriate TaskListView. Otherwise, loop back to the Log In page.
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
        // Set up the Create User page.
        public IActionResult CreateUser()
        {
            ViewBag.logMsg = "Ready";
            return View();
        }

        // On form submit, take the input and see if there is already an existing user with the given email. 
        //   If so, loop back to Create User without doing anything aside from updating the log message.
        //   Otherwise, as long as the input criteria are met (or else nothing happens), create a new User entry for the database. 
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
        // Ready and display the TaskListView, which shows a list of ToDoItems that belong to the user that is logged in.
        // Depending on the given string for a sortOrder, the list of ToDoItems will be organized by ID by default, but can be organized by their
        //   Deadline or IsDone values.
        // The view is reloaded everytime the "Go!" button above the table is clicked.
        // ToDoItems that are past the given date will be different visual-wise compared to other entries.
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

        // Clicking on a ToDoItem's "Toggle" button will toggle the IsDone value from true to false, and vice versa.
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

        // Clicking on a ToDoItem's "Delete" button will remove the entry from the list.
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
        // Sets up the CreateTask view while grabbing the current User's ID for filling out new ToDoItem values (hidden inputs)
        public IActionResult CreateTask()
        {
            User U = _db.Users.Find(TempData["uID"]);
            ViewBag.CurrentUser = U;
            TempData["uID"] = U.Id;
            return View();
        }

        // On form post, create a new ToDoItem and add it to the ToDoItems table on the database, and then redirect back to the user's appropriate TaskListView.
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
