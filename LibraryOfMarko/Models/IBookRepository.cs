using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryOfMarko.Models
{
    public interface IBookRepository
    {
        void AddBook(Book newBook);
        void RemoveBook(int id);
        void EditBook(Book editedBook);
        Book GetBook(int id);
        List<Book> SearchBook(string query);
        void RentBook(int bookId, int userId);
        void ReturnBook(int bookId, int userId);
        bool IsBookAvailable(Book book);
        List<Book> MostRentedBooks();
    }
}
