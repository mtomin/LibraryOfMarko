using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryOfMarko.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOfMarko.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _iUserRepository;
        public UsersController(IUserRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "List of users";
            List<User> model = _iUserRepository.GetAllUsers();
            return View(model);
        }
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.Title = "Search Users";
            return View();
        }
        [HttpPost]
        public IActionResult Search(string query)
        {
            ViewBag.Title = "Search Results";
            List<User> model = _iUserRepository.SearchUser(query);
            return View(model);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.Title = "Add new user";
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(User newUser)
        {
            ViewBag.Title = "Add new user";
            if (ModelState.IsValid)
            {
                _iUserRepository.AddUser(newUser);
                return RedirectToAction("UserDetails", new { id = newUser.ID });
            }
            return View();
        }

        [Route("Users/UserDetails/{id}")]
        public IActionResult UserDetails(int id)
        {
            List<UserData> model = _iUserRepository.LoadUserData(id);
            ViewBag.Title = String.Format($"{model[0].FirstName} {model[0].LastName}");
            return View(model);
        }
        public IActionResult RemoveUser(int userid)
        {
            _iUserRepository.RemoveUser(userid);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditUser(int userId)
        {
            User model = _iUserRepository.GetUser(userId);
            ViewBag.Title = String.Format($"Editing user {model.FirstName} {model.LastName}");
            return View(model);
        }
        [HttpPost]
        [ActionName("EditUser")]
        public IActionResult UpdateUser(User editedUser)
        {
            if (ModelState.IsValid)
            {
                _iUserRepository.EditUser(editedUser);
                return RedirectToAction("UserDetails", new { id = editedUser.ID });
            }
            return View(editedUser);
        }
    }
}