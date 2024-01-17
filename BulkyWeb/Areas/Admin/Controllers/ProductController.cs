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

        public ProductController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});
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
                _unitOfWork.Product.Add(productVM.Product);
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
