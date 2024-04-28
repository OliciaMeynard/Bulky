using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleManagementVM RoleManagementVM { get; set; }

        public UserController(ApplicationDbContext db, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {

            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            

        }
        public IActionResult Index()
        {


            return View();
        }

       
        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach(var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

             

                if (user.Company == null) {
                    user.Company = new() { Name = "" };

                }
            }
            return Json(new {data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while locking/unlocking" });
            }

            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and need to unlock
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(2000);
            }

            _db.SaveChanges();

            return Json(new { success = true, message = "Operation Successful" });
        }

        public IActionResult RoleManagement(string userId) {

            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            RoleManagementVM = new()
            {
                ApplicationUser = _db.ApplicationUsers.Include( u=> u.Company).FirstOrDefault(u => u.Id == userId),
                CompanyList = _db.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                RoleList = _db.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                }),

            };

            RoleManagementVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;


            return View(RoleManagementVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleVM)
        {


            string roleID = _db.UserRoles.FirstOrDefault(u => u.UserId == roleVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == roleID).Name;

            if (!(roleVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleVM.ApplicationUser.Id);
                if(roleVM.ApplicationUser.Role == SD.Role_Company)
                {

                    applicationUser.CompanyId = roleVM.ApplicationUser.CompanyId;

                }
                if(oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }

                _db.SaveChanges();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }



            return RedirectToAction("Index");
        }
        #endregion

    }
}

