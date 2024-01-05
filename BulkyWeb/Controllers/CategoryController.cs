using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        ///this is controller method
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        ///this is action method
        public IActionResult Index()
        {
            //var objCategoryList = _db.Categories.ToList();
            //or use this
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            ////this is for custom validation
            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "test")
            {
                //ModelState.AddModelError("Name", "Hindi pwede ang test");
                ModelState.AddModelError("", "Hindi pwede ang test");
            }

            //if (categoryObj.Name.ToLower() == null && categoryObj.DisplayOrder.ToString() == null)
            //{
            //    ModelState.AddModelError("", "Wag blank! loko");
            //}


            if (ModelState.IsValid)
            {
            _db.Categories.Add(categoryObj);
            _db.SaveChanges();
            return RedirectToAction("Index");

            }

            return View();
            //if in different controller do this
            // return RedirectToAction("Index", "differentController");
        }








        //////THIS IS MY JQUERY SAMPLE CODE, THIS IS WORKING
        //[HttpGet]
        //public JsonResult GetList()
        //{
        //    var objCategoryList = _db.Categories.ToList();
        //    return new JsonResult(Ok(objCategoryList));
        //}


        //[HttpPost]
        //public JsonResult PostObject(string name, string job, int age)
        //{


        //    var thename = name;
        //    var thejob = job;
        //    var theage = age;
        //    return new JsonResult(Ok(name));
        //}
    }
}
