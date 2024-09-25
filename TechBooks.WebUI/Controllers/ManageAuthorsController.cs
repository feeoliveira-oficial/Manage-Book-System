using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using TechBooks.Data.ADO.Net;
using TechBooks.Models;

namespace TechBooks.WebUI.Controllers
{
    [Authorize]
    public class ManageAuthorsController : CommonBaseClass
    {
        
        public ActionResult Index()
        {
            var listOfAuthors = new List<Author>();
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    listOfAuthors = AuthorsData.GetList(cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }
            return View(listOfAuthors);
        }

        public ActionResult AddOrUpdate(int? id)
        {
            if (id == null) return View();

            Author author = null;
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    author = AuthorsData.GetAuthor((int)id, cn);
                    if (author == null)
                        return RedirectToAction("Index", "NotFound", new { entity = "Author", backUrl = "/ManageAuthors/" });
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate(Author author)
        {
            if (!ModelState.IsValid)
                return (author.AuthorId == 0) ? View() : View(author);

            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (author.AuthorId == 0)
                        AuthorsData.Insert(author, cn);
                    else
                        AuthorsData.Update(author, cn);

                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (author.AuthorId == 0) ? View() : View(author);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(Author author)
        {
            try
            {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    if (AuthorsData.HasBooks(author, cn))
                        throw new Exception("This Author cannot be removed because it has been associated with one or more books. Remove all associations first.");
                    else
                        AuthorsData.Delete(author, cn);
                }
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}