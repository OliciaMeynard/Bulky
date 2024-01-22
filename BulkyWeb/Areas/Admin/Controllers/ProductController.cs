using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {

            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {

            ///for drop down category
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ///for drop down category
            ///

            ProductVM producVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select( u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            ////CREATE
            if(id == null || id == 0)
            {
            return View(producVM);
            }


            ////UPDATE
            else
            {
                producVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                return View(producVM);
            }
        }


        ////IFormFile is for file upload
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        ////IFormFile is for file upload
        {
            if (ModelState.IsValid)
            {
                 ////wwwroot folder
                string wwwRootpath = _webHostEnvironment.WebRootPath;



                ////////FILE UPLOAD
                if (file != null)
                {
                    //string fileName = Guid.NewGuid().ToString() + file.FileName.ToString() + Path.GetExtension(file.FileName);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootpath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootpath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                ////////FILE UPLOAD





                ////if it is null
                if (file == null)
                {
                    if (string.IsNullOrEmpty(productVM.Product.ImageUrl)){
                    
                        productVM.Product.ImageUrl = "";
                    }
                }
                ////if it is null


  



                ///add
                if (productVM.Product.Id == 0) {
                
                     _unitOfWork.Product.Add(productVM.Product);
                }
                //update
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }
        }

        ////OLD CREATE CODE WITHOUT VM
        //public IActionResult Create(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(product);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

     

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Product objProductFromDb = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

            if(objProductFromDb == null)
            {
                return NotFound();
            }

            return View(objProductFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int? id)
        {
            Product objproduct = _unitOfWork.Product.GetFirstOrDefault(y => y.Id == id);
            if(objproduct == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(objproduct);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult DeleteAPI(int? id)
        {
            var productToDelete = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if(productToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            var oldImagePath =  Path.Combine(_webHostEnvironment.WebRootPath, productToDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}


//////////// OLD CODE WHEN CREATE AND UPDATE IS NOT YET COMBINED
///using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBook.Models;
//using BulkyBook.Models.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Collections.Generic;

//namespace BulkyBookWeb.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public ProductController(IUnitOfWork unitOfWork)
//        {

//            _unitOfWork = unitOfWork;
//        }
//        public IActionResult Index()
//        {
//            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
//            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
//            //{
//            //    Text = u.Name,
//            //    Value = u.Id.ToString()
//            //});
//            return View(objProductList);
//        }

//        public IActionResult Create()
//        {
//            ///for drop down category
//            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
//            //{
//            //    Text = u.Name,
//            //    Value = u.Id.ToString()
//            //});
//            //ViewBag.CategoryList = CategoryList;
//            //ViewData["CategoryList"] = CategoryList;
//            ///for drop down category
//            ///
//            ProductVM producVM = new()
//            {
//                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
//                {
//                    Text = u.Name,
//                    Value = u.Id.ToString()
//                }),
//                Product = new Product()
//            };

//            return View(producVM);
//        }

//        [HttpPost]
//        public IActionResult Create(ProductVM productVM)
//        {
//            if (ModelState.IsValid)
//            {
//                _unitOfWork.Product.Add(productVM.Product);
//                _unitOfWork.Save();
//                TempData["success"] = "Product created successfully";
//                return RedirectToAction("Index");
//            }

//            else
//            {

//                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
//                {
//                    Text = u.Name,
//                    Value = u.Id.ToString()
//                });

//                return View(productVM);
//            }
//        }

//        ////OLD CREATE CODE WITHOUT VM
//        //public IActionResult Create(Product product)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        _unitOfWork.Product.Add(product);
//        //        _unitOfWork.Save();
//        //        TempData["success"] = "Product created successfully";
//        //        return RedirectToAction("Index");
//        //    }
//        //    return View();
//        //}

//        public IActionResult Edit(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                return NotFound();
//            }

//            Product? productFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
//            if (productFromDb == null)
//            {
//                return NotFound();
//            }
//            return View(productFromDb);

//        }

//        [HttpPost]
//        public IActionResult Edit(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                _unitOfWork.Product.Update(product);
//                _unitOfWork.Save();
//                TempData["success"] = "Product updated successfully";
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

//            Product objProductFromDb = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);

//            if (objProductFromDb == null)
//            {
//                return NotFound();
//            }

//            return View(objProductFromDb);
//        }

//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeleteProduct(int? id)
//        {
//            Product objproduct = _unitOfWork.Product.GetFirstOrDefault(y => y.Id == id);
//            if (objproduct == null)
//            {
//                return NotFound();
//            }
//            _unitOfWork.Product.Remove(objproduct);
//            _unitOfWork.Save();
//            return RedirectToAction("Index");
//        }
//    }
//}
