using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IRepository<Book> _book;
        private readonly IRepository<Author> _author;

        public BookController(IRepository<Book> book, IRepository<Author> author)
        {
            _book = book;
            _author = author;
        }

        public IActionResult Index()
        {
            return View(_book.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            try
            {
                var model = id == null
                ? new Book()
                : _book.Get((int)id);

                FillAuthorSelectBox();
                ViewData["result"] = true;
                return View(model);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

       

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Book model)
        {
            ViewData["result"] = true;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                FillAuthorSelectBox();

                if (model.BookId == 0)
                    _book.Insert(model);
                else
                    ViewData["result"] = _book.Update(model);

                if ((bool)ViewData["result"])
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Book model = _book.Get((int)id);
                return View(model);

            }
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                _book.Delete((int)id);
                return RedirectToAction("Index");
            }
        }

        private void FillAuthorSelectBox()
        {
            var author = _author
              .GetAll()
              .Select(x => new AuthorViewModel
              {
                  AuthorId = x.AuthorId,
                  FullName = $"{x.Name} {x.Surname}",
              });
            ViewData["authors"] = author;
        }
    }
}