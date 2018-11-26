using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class AuthorBook
    {
        //[Key]
        //public int AuthorBookId { get; set; }

        public int? AuthorId { get; set; }
        public Author Author { get; set; }

        public int? BookId { get; set; }
        public Book Book { get; set; }

    }
}
