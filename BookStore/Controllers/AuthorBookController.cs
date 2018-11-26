using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorBookController : Controller
    {
        private readonly IRepository<AuthorBook> _authorBook;
        private readonly IRepository<Author> _author;
        private readonly IRepository<Book> _book;


        public AuthorBookController(IRepository<AuthorBook> authorBook, IRepository<Author> author, IRepository<Book> book)
        {
            _authorBook = authorBook;
            _author = author;
            _book = book;
        }

        public IActionResult Index()
        {
            var AuthorBooksList = _authorBook.GetAll();

            var AutorBookViewModel = AuthorBooksList.GroupBy(x => x.Author.Name)
                .Select(c => new AuthorBookViewModel
                {
                    AuthorName = c.Key,
                    Books = c.Select(x => x.Book).ToList(),
                });
          
            return View(AutorBookViewModel);
        }

        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    try
        //    {
        //        var model = id == null
        //        ? new AuthorBook()
        //        : _authorBook.Get((int)id);
        //        ViewBag.Book = _book.GetAll();
        //        ViewBag.Author = _author.GetAll();
        //        return View(model);
        //    }
        //    catch (NotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpPost]
        //public IActionResult Edit(AuthorBook model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(model);
        //        }

        //        if (model.AuthorBookId == 0)
        //            _authorBook.Insert(model);
        //        else
        //            _authorBook.Update(model);

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (NotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        AuthorBook model = _authorBook.Get((int)id);
        //        return View(model);

        //    }
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirm(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        _authorBook.Delete((int)id);
        //        return RedirectToAction("Index");
        //    }
        //}

    }
}