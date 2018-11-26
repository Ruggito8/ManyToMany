using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.DataAccess
{
    public class SqlRepository : IdentityDbContext, IRepository<Book>, IRepository<Author>, IRepository<AuthorBook>
    {
        public SqlRepository(DbContextOptions options) : base (options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorBook>()
                .HasKey(t => new { t.AuthorId, t.BookId });
        }

        void IRepository<Book>.Delete(int id)
        {
            Book dbItem = Books.FirstOrDefault(x => x.BookId == id);
            Remove(dbItem);
            SaveChanges();
        }

        void IRepository<Author>.Delete(int id)
        {
            Author dbItem = Authors.FirstOrDefault(x => x.AuthorId == id);
            Remove(dbItem);
            SaveChanges();
        }

        void IRepository<AuthorBook>.Delete(int id)
        {
            AuthorBook dbItem = AuthorBooks.FirstOrDefault(x => x.AuthorId == id);
            Remove(dbItem);
            SaveChanges();
        }

        Book IRepository<Book>.Get(int id)
        {
            return Books.FirstOrDefault(x => x.BookId == id);
        }

        Author IRepository<Author>.Get(int id)
        {
            return Authors.FirstOrDefault(x => x.AuthorId == id);
        }

        AuthorBook IRepository<AuthorBook>.Get(int id)
        {
            return AuthorBooks.FirstOrDefault(x => x.AuthorId == id);
        }

        List<Book> IRepository<Book>.GetAll()
        {
            return Books.ToList();
        }

        List<Author> IRepository<Author>.GetAll()
        {
            var list = Authors.Include(a => a.AuthorBooks).
               ThenInclude(a => a.Book).ToList();

            return list;
        }

        List<AuthorBook> IRepository<AuthorBook>.GetAll()
        {
            return AuthorBooks.Include(a => a.Author).Include(a => a.Book).ToList();
        }

        int IRepository<Book>.Insert(Book model)
        {
           Books.Add(model);
           return SaveChanges();
        }

        int IRepository<Author>.Insert(Author model)
        {
            Authors.Add(model);
            return SaveChanges();
        }

        int IRepository<AuthorBook>.Insert(AuthorBook model)
        {
            AuthorBooks.Add(model);
            return SaveChanges();
        }

        bool IRepository<Book>.Update(Book model)
        {
            var authorIds = model.RelatedAuthorIds;
            var bookId = model.BookId;
            
            if(authorIds.Contains(-1) && authorIds.Count > 1)
            {
                return false;
            }

            var delete = AuthorBooks.Where(x => x.BookId == bookId);

            AuthorBooks.RemoveRange(delete);

            var insert = authorIds.Where(id=> id != -1).Select(x =>new AuthorBook
            {
                AuthorId = x,
                BookId = bookId,
                Author = null,
                Book = null,
            });

            AuthorBooks.AddRange(insert);

            Books.Update(model);
            SaveChanges();
            return true;
        }

        bool IRepository<Author>.Update(Author model)
        {
            var authorId = model.AuthorId;
            var bookIds = model.RelatedBookIds;

            if (bookIds.Contains(-1) && bookIds.Count > 1)
            {
                return false;
            }

            var delete = AuthorBooks.Where(x => x.AuthorId == authorId);

            AuthorBooks.RemoveRange(delete);

            var insert = bookIds.Where(id => id != -1).Select(x => new AuthorBook
            {
                AuthorId = authorId,
                BookId = x,
                Author = null,
                Book = null,
            });

            AuthorBooks.AddRange(insert);

            Authors.Update(model);
            SaveChanges();
            return true;
        }

        bool IRepository<AuthorBook>.Update(AuthorBook model)
        {
            AuthorBooks.Update(model);
            SaveChanges();
            return true;
        }
    }
}
