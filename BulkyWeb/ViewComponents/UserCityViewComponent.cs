using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents
{
    public class UserCityViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserCityViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
   

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                 var userDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                 HttpContext.Session.SetString(SD.SessionUserCity, userDb.City);
                 return View(HttpContext.Session.GetString(SD.SessionUserCity));
            }
            else
            {
                HttpContext.Session.Clear();
                return View("");
            }
        }
    }
}
