using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
           IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category"),
                Count = 1,
                ProductId = id
            };
            //Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category");
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            ///getting users ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault( c => c.ApplicationUserId == userId && c.ProductId == shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                ///// shopping cart already exist
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            else
            {
                //add cart record
            _unitOfWork.ShoppingCart.Add(shoppingCart);

            }

            _unitOfWork.Save();
            TempData["success"] = "Product has been added to your cart";


            //return RedirectToAction("Index");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
