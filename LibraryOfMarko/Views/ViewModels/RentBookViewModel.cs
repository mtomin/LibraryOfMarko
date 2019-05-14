using LibraryOfMarko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryOfMarko.Views.ViewModels
{
    public class RentBookViewModel
    {
        public Book Book { get; set; }
        public List<User> Users { get; set; }
        public int SelectedUserId { get; set; }
    }
}
