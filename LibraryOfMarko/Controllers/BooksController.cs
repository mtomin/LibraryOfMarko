using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryOfMarko.Models;
using LibraryOfMarko.Views.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOfMarko.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _iBookRepository;
        private readonly IUserRepository _iUserRepository;
        private readonly IHostingEnvironment _iHostingEnvironment;

        public BooksController(IBookRepository iBookRepository, IUserRepository iUserRepository, IHostingEnvironment iHostingEnvironment)
        {
            _iBookRepository = iBookRepository;
            _iUserRepository = iUserRepository;
            _iHostingEnvironment = iHostingEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "Books";
            List<Book> model = _iBookRepository.MostRentedBooks();
            return View(model);
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
        public IActionResult AddBook(AddBookViewModel newBookInfo)
        {
            ViewBag.Title = "Book added";
            if (ModelState.IsValid)
            {
                Book book = newBookInfo.Book;
                if (newBookInfo.Cover != null)
                {
                    string uniqueFilename = Guid.NewGuid() + "_" + newBookInfo.Cover.FileName;
                    book.CoverPath = uniqueFilename;
                    using (var fs = new FileStream(Path.Combine(_iHostingEnvironment.WebRootPath, "images", uniqueFilename), FileMode.Create))
                        newBookInfo.Cover.CopyTo(fs);
                }
                _iBookRepository.AddBook(book);
                return RedirectToAction("BookDetails", new { bookID = book.ID });
            }
            return View();
        }
        [HttpGet]
        public IActionResult BookDetails(int bookID)
        {
            Book model = _iBookRepository.GetBook(bookID);
            ViewBag.Title = model.Title;
            if (model.CoverPath != null)
            {
                model.CoverPath = Path.Combine("~/images", model.CoverPath);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditBook(int bookID)
        {
            AddBookViewModel model = new AddBookViewModel
            {
                Book = _iBookRepository.GetBook(bookID)
            };
            ViewBag.Title = String.Format($"Editing {model.Book.Title}");
            return View(model);
        }
        [HttpPost]
        [ActionName("EditBook")]
        public IActionResult EditBookUpdate(AddBookViewModel editedBook)
        {
            if (ModelState.IsValid)
            {
                if (editedBook.Cover != null)
                {
                    string uniqueFilename = Guid.NewGuid() + "_" + editedBook.Cover.FileName;
                    editedBook.Book.CoverPath = uniqueFilename;
                    using (var fs = new FileStream(Path.Combine(_iHostingEnvironment.WebRootPath, "images", uniqueFilename), FileMode.Create))
                        editedBook.Cover.CopyTo(fs);
                }
                ViewBag.Title = String.Format($"{editedBook.Book.Title} sucessfully edited!");
                _iBookRepository.EditBook(editedBook.Book);
                return RedirectToAction("BookDetails", new { bookID = editedBook.Book.ID });
            }
            return View();
        }
        [Route("Books/RentBook/{bookId}")]
        [HttpGet]
        public IActionResult RentBook(int bookId, string user)
        {
            Book book = _iBookRepository.GetBook(bookId);
            if (_iBookRepository.IsBookAvailable(book))
            {
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
            else
            {
                ViewBag.Title = "Book currently not available";
                return View("RentBookFailed", book);
            }
        }

        public IActionResult RentBookUpdateDB(int bookId, int selectedUserId)
        {
            _iBookRepository.RentBook(bookId, selectedUserId);
            return RedirectToAction("UserDetails", "Users", new { id = selectedUserId });
        }
        public IActionResult ReturnBook(int bookId, int userId)
        {
            _iBookRepository.ReturnBook(bookId, userId);
            return RedirectToAction("UserDetails", "Users", new { id = userId });
        }
    }
}