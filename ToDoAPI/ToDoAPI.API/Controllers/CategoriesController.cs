using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF; //to connect in to EF layer
using ToDoAPI.API.Models; //acces to the DTO's
using System.Web.Http.Cors; 

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        public IHttpActionResult GetCategories()
        {
            List<CategoryViewModel> cats = db.Categories.Select(r => new CategoryViewModel()
            {
                CategoryId = r.CategoryId,
                Description = r.Description,
                Name = r.Name
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
            {
                return NotFound();
            }

            return Ok(cats);
        }//end GetCategories()

        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(r => r.CategoryId == id).Select(r => new CategoryViewModel()
            {
                CategoryId = r.CategoryId,
                Description = r.Description,
                Name = r.Name
            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }//end GetCategory()

        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            Category newCategory = new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            };

            db.Categories.Add(newCategory);
            db.SaveChanges();
            return Ok(newCategory);
        }//end PostCategory()


        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");

            Category existingCat = db.Categories.Where(r => r.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.CategoryId = cat.CategoryId;
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutCategory()
        
        
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(r => r.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end DeleteCategory()

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}