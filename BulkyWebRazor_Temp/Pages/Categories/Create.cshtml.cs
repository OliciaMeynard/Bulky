using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        //this bind property is working if you want to bind individually if you have multiple methods
        //[BindProperty]
        public Category Category{ get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }


        public void OnGet()
        {
        }


        //no need to add parameter in OnPost method because category is binded   [BindProperty]
        public IActionResult OnPost()
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}
