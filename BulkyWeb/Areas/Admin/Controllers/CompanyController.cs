using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            //List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {



            ////CREATE
            if(id == null || id == 0)
            {
            return View(new Company());
            }


            ////UPDATE
            else
            {
                Company companyObj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
                return View(companyObj);
            }
        }


        ////IFormFile is for file upload
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        ////IFormFile is for file upload
        {
            if (ModelState.IsValid)
            {


                ///add
                if (CompanyObj.Id == 0) {
                
                     _unitOfWork.Company.Add(CompanyObj);
                }
                //update
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }

            else
            {

                return View(CompanyObj);
            }
        }


     

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Company objCompanyFromDb = _unitOfWork.Company.GetFirstOrDefault(p => p.Id == id);

            if(objCompanyFromDb == null)
            {
                return NotFound();
            }

            return View(objCompanyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCompany(int? id)
        {
            Company objCompany = _unitOfWork.Company.GetFirstOrDefault(y => y.Id == id);
            if(objCompany == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(objCompany);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = objCompanyList});
        }

        [HttpDelete]
        public IActionResult DeleteAPI(int? id)
        {
            var CompanyToDelete = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if(CompanyToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }



            _unitOfWork.Company.Remove(CompanyToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

