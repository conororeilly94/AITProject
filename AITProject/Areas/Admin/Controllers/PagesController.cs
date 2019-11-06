using AITProject.Models.Data;
using AITProject.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AITProject.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of Page View Models
            List<PageVM> pagesList;

            //Init the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //Return view with list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // GET: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Declare slug
                string slug;

                //Init pageDTO
                PageDTO dto = new PageDTO();

                //DTO title
                dto.Title = model.Title;

                //Check for and set slug if needed
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "You have added a new page";

            //Redirect
            return RedirectToAction("Add Page");
        }

        //GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare pageVM
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                }

                //Init pageVM
                model = new PageVM(dto);
            }

            //Return view with model
            return View(model);
        }

        //POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // Check model state

            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                //Get page id
                int id = model.Id;

                //Init slug
                string slug = "home";

                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Dto the title
                dto.Title = model.Title;

                //Check for slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();

                    }
                }

                //Make sure title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                        ModelState.AddModelError("", "That title or slug already exists");
                        return View(model);
                }

                //Dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //Save the dto
                db.SaveChanges();
            }

            //Set TempData message
            TempData["SM"] = "You have edited the page";

            //Redirect
            return RedirectToAction("Edit Page");
        }

        //GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist");
                }

                //Init PageVM
                model = new PageVM(dto);
            }

            //Return view with model
            return View(model);
        }

        //GET: Admin/Pages/DeletePages/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Remove the page
                db.Pages.Remove(dto);

                //Save
                db.SaveChanges();
            }

            //Redirect
            return RedirectToAction("Index");
        }
    }
}