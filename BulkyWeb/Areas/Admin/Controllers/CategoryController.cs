
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    [Authorize(Roles = $"{SD.Role_Employee},{SD.Role_Admin}")]
    public class CategoryController : Controller
    {
        //private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        ///this is controller method
        //public CategoryController(ICategoryRepository db)
        //public CategoryController(IUnitOfWork db)
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        ///this is action method
        public IActionResult Index(List<Category>? objCategoryListt)
        {
            //var objCategoryList = _db.Categories.ToList();
            //or use this
            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }



        /// SEARCH
        [HttpPost]
        public IActionResult Search(string searchString)
        {
            List<Category> objCategoryListt = _unitOfWork.Category.GetAll().ToList();
            if (!string.IsNullOrEmpty(searchString)) {
                var filteredResult = objCategoryListt.Where(n => n.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                //var filteredResult = objCategoryList.Where(n => n.Name.Contains(searchString) || n.any.contains(searchString));
                return View("Index", filteredResult);
            }
            else
            {
            return View("Index", objCategoryListt);

            }
        }

        /// SEARCH


        //[Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            ////this is for custom validation
            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "testtingnga")
            {
                //ModelState.AddModelError("Name", "Hindi pwede ang test");
                ModelState.AddModelError("Name", "Hindi pwede ang testtingnga");
                //ModelState.AddModelError("", "Hindi pwede ang testtingnga");
            }

            //if (categoryObj.Name.ToLower() == null && categoryObj.DisplayOrder.ToString() == null)
            //{
            //    ModelState.AddModelError("", "Please fill required fields");
            //}


            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(categoryObj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");

            }

            return View();
            //if in different controller do this
            // return RedirectToAction("Index", "differentController");
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //ways to retrieve data record
            //1
            //Category categoryFromDb = _db.Categories.Find(id);

            //2 //question mark is for nullable it's optional
            Category? categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(cat => cat.Id == id);

            //3
            //Category? categoryFromDb = _db.Categories.Where(cat => cat.Id == id).FirstOrDefault();




            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);

        }


        [HttpPost]
        public IActionResult Edit(Category categoryObj)
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
                _unitOfWork.Category.Update(categoryObj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");

            }

            return View();



        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(cat => cat.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);

        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {


            Category categoryObj = _unitOfWork.Category.GetFirstOrDefault(cat => cat.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }


    }
}





/////////////////////////////////////////
//////////////////OLD CODE
///

////using BulkyBookWeb.Data;
////using BulkyBookWeb.Models;
//using BulkyBook.DataAccess.Data;
//using BulkyBook.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace BulkyBookWeb.Controllers
//{
//    public class CategoryController : Controller
//    {
//        private readonly ApplicationDbContext _db;

//        ///this is controller method
//        public CategoryController(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        ///this is action method
//        public IActionResult Index()
//        {
//            //var objCategoryList = _db.Categories.ToList();
//            //or use this
//            List<Category> objCategoryList = _db.Categories.ToList();
//            return View(objCategoryList);
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }


//        [HttpPost]
//        public IActionResult Create(Category categoryObj)
//        {
//            ////this is for custom validation
//            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "test")
//            {
//                //ModelState.AddModelError("Name", "Hindi pwede ang test");
//                ModelState.AddModelError("", "Hindi pwede ang test");
//            }

//            //if (categoryObj.Name.ToLower() == null && categoryObj.DisplayOrder.ToString() == null)
//            //{
//            //    ModelState.AddModelError("", "Please fill required fields");
//            //}


//            if (ModelState.IsValid)
//            {
//                _db.Categories.Add(categoryObj);
//                _db.SaveChanges();
//                TempData["success"] = "Category created successfully";
//                return RedirectToAction("Index");

//            }

//            return View();
//            //if in different controller do this
//            // return RedirectToAction("Index", "differentController");
//        }


//        public IActionResult Edit(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }

//            //ways to retrieve data record
//            //1
//            //Category categoryFromDb = _db.Categories.Find(id);

//            //2 //question mark is for nullable it's optional
//            Category? categoryFromDb = _db.Categories.FirstOrDefault(record => record.Id == id);

//            //3
//            //Category? categoryFromDb = _db.Categories.Where(cat => cat.Id == id).FirstOrDefault();




//            if (categoryFromDb == null)
//            {
//                return NotFound();
//            }

//            return View(categoryFromDb);

//        }


//        [HttpPost]
//        public IActionResult Edit(Category categoryObj)
//        {

//            ////this is for custom validation
//            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "test")
//            {
//                //ModelState.AddModelError("Name", "Hindi pwede ang test");
//                ModelState.AddModelError("", "Hindi pwede ang test");
//            }

//            //if (categoryObj.Name.ToLower() == null && categoryObj.DisplayOrder.ToString() == null)
//            //{
//            //    ModelState.AddModelError("", "Wag blank! loko");
//            //}


//            if (ModelState.IsValid)
//            {
//                _db.Categories.Update(categoryObj);
//                _db.SaveChanges();
//                TempData["success"] = "Category updated successfully";
//                return RedirectToAction("Index");

//            }

//            return View();



//        }


//        public IActionResult Delete(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }

//            Category categoryFromDb = _db.Categories.Find(id);

//            if (categoryFromDb == null)
//            {
//                return NotFound();
//            }

//            return View(categoryFromDb);

//        }

//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeletePOST(int? id)
//        {


//            Category categoryObj = _db.Categories.Find(id);
//            if (categoryObj == null)
//            {
//                return NotFound();
//            }

//            _db.Categories.Remove(categoryObj);
//            _db.SaveChanges();
//            TempData["success"] = "Category deleted successfully";
//            return RedirectToAction("Index");
//        }








//        //////THIS IS MY JQUERY SAMPLE CODE, THIS IS WORKING
//        //[HttpGet]
//        //public JsonResult GetList()
//        //{
//        //    var objCategoryList = _db.Categories.ToList();
//        //    return new JsonResult(Ok(objCategoryList));
//        //}


//        //[HttpPost]
//        //public JsonResult PostObject(string name, string job, int age)
//        //{


//        //    var thename = name;
//        //    var thejob = job;
//        //    var theage = age;
//        //    return new JsonResult(Ok(name));
//        //}

//        //[HttpPost]
//        //public JsonResult CreateCategory(Category category)
//        //{
//        //    var newcat = category;
//        //    newcat.Name = category.Name;
//        //    newcat.DisplayOrder = (int)category.DisplayOrder;
//        //    if(category != null)
//        //    {
//        //        _db.Categories.Add(newcat);
//        //        _db.SaveChanges();
//        //        //return RedirectToAction(")
//        //    }
//        //    return Json("Ok");
//        //}
//    }
//}


