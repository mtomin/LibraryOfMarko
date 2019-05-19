using LibraryOfMarko.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryOfMarko.Views.ViewModels
{
    public class AddBookViewModel
    {
        public Book Book { get; set; }
        public IFormFile Cover { get; set; }
    }
}
