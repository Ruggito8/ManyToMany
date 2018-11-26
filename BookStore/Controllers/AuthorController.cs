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
    public class AuthorController : Controller
    {
        private readonly IRepository<Author> _author;
        private readonly IRepository<Book> _book;
        

        public AuthorController(IRepository<Author> author, IRepository<Book> book)
        {
            _author = author;
            _book = book;
        }

        public IActionResult Index()
        {
            return View(_author.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            try
            {
                var model = id == null
                ? new Author()
                : _author.Get((int)id);

                FillBookSelectBox();

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
        public IActionResult Edit(Author model)
        {
            ViewData["result"] = true;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                FillBookSelectBox();

                if (model.AuthorId == 0)
                    _author.Insert(model);
                else
                    ViewData["result"] = _author.Update(model);

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

        private void FillBookSelectBox()
        {
            var books = _book
              .GetAll()
              .Select(x => new BookViewModel
              {
                  BookId = x.BookId,
                  Title = x.Name
              });
            ViewData["books"] = books;
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
                Author model = _author.Get((int)id);
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
                _author.Delete((int)id);
                return RedirectToAction("Index");
            }
        }

    }
}