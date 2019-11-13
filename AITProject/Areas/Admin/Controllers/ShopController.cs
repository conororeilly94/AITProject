using AITProject.Models.Data;
using AITProject.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AITProject.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Declare list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                //Init the list
                categoryVMList = db.Categories.
                    ToArray().
                    OrderBy(x => x.Sorting).
                    Select(x => new CategoryVM(x)).
                    ToList();
            }

            //Return view with list
            return View(categoryVMList);
        }

        // POST: Admin/Shop/AddNewCategories
        [HttpPost]
        public string AddNewCategories(string catName)
        {
            //Declare id
            string id;

            using (Db db = new Db())
            {
                //Check category name is unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                //Init DTO
                CategoryDTO dto = new CategoryDTO();

                //Add to DTO
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                //Save to DTO
                db.Categories.Add(dto);
                db.SaveChanges();

                //Get the id
                id = dto.Id.ToString();
            }

            //Return id
            return id;
        }

        //GET: Admin/Pages/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //Set inital count
                int count = 1;

                //Declare pageDTO
                CategoryDTO dto;

                //Set sorting for each category
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }
    }
}