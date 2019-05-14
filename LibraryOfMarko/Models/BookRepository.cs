﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace LibraryOfMarko.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;
        public BookRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("LibraryDB");
        }
        public void AddBook(Book newBook)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                newBook.ID = db.Query<int>("AddBook @Title, @Author, @CopiesAvailable", newBook).SingleOrDefault();
            }
        }

        public void EditBook(Book editedBook)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("EditBook", editedBook, commandType: CommandType.StoredProcedure);
            }
        }

        public Book GetBook(int id)
        {
            Book book = new Book();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                book = db.Query<Book>("GetBook", new { @Id = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
            return book;
        }

        public void RemoveBook(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("RemoveBook", new { @Id = id }, commandType: CommandType.StoredProcedure);
            }
        }

        public void RentBook(int bookId, int userId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("RentBook", new { @UserId=userId, @BookId=bookId, @DateRented=DateTime.Now }, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Book> SearchBook(string query)
        {
            List<Book> books = new List<Book>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                books = db.Query<Book>("SearchBooks", new { query = query.ToLower() }, commandType: CommandType.StoredProcedure).ToList();
            }
            return books;
        }
    }
}
