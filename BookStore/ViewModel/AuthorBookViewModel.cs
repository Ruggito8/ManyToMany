using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModel
{
    public class AuthorBookViewModel
    {
        public string AuthorName { get; set; }
        public List<Book> Books { get; set; }
    }
}
