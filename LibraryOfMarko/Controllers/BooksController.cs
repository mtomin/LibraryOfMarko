using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryOfMarko.Models;
using LibraryOfMarko.Views.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOfMarko.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _iBookRepository;
        private readonly IUserRepository _iUserRepository;
        public BooksController(IBookRepository iBookRepository, IUserRepository iUserRepository)
        {
            _iBookRepository = iBookRepository;
            _iUserRepository = iUserRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Books";
            return View();
        }
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.Title = "Search books";
            return View();
        }
        [HttpPost]
        public IActionResult Search(string query)
        {
            ViewBag.Title = "Search books";
            List<Book> model = _iBookRepository.SearchBook(query);
            return View(model);
        }
        [HttpGet]
        public IActionResult AddBook()
        {
            ViewBag.Title = "Add new bok";
            return View();
        }
        [HttpPost]
        public IActionResult AddBook(Book newBook)
        {
            ViewBag.Title = "Book added";
            if (ModelState.IsValid)
            {
                _iBookRepository.AddBook(newBook);
                return RedirectToAction("BookDetails", new { bookID = newBook.ID });
            }
            return View();
        }
        [Route("Books/BookDetails/{bookID}")]
        public IActionResult BookDetails(int bookID)
        {
            ViewBag.Title = "Add new book";
            Book model = _iBookRepository.GetBook(bookID);
            return View(model);
        }
        [HttpGet]
        public IActionResult EditBook(int bookID)
        {
            Book editedBook = _iBookRepository.GetBook(bookID);
            ViewBag.Title = String.Format($"Editing {editedBook.Title}");
            return View(editedBook);
        }
        [HttpPost]
        [ActionName("EditBook")]
        public IActionResult EditBookUpdate(Book editedBook)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Title = String.Format($"{editedBook.Title} sucessfully edited!");
                _iBookRepository.EditBook(editedBook);
                return RedirectToAction("BookDetails", new { bookID = editedBook.ID });
            }
            return View();
        }
        [Route("Books/RentBook/{bookId}")]
        [HttpGet]
        public IActionResult RentBook(int bookId, string user)
        {
            Book book = _iBookRepository.GetBook(bookId);
            List<User> users = new List<User>();
            if (user != null)
            {
                users = _iUserRepository.SearchUser(user);
            }

            ViewBag.Title = String.Format($"Rent {book.Title}");
            var RentBookViewModel = new RentBookViewModel
            {
                Book = book,
                Users = users
            };
            return View(RentBookViewModel);
        }
        //[Route("Books/RentBook/{bookId}")]
        public IActionResult RentBookUpdateDB(int bookId, int selectedUserId)
        {
            _iBookRepository.RentBook(bookId, selectedUserId);
            return RedirectToAction("UserDetails", "Users", new { id = selectedUserId });
        }
    }
}