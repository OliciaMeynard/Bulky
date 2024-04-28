using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using BulkyBook.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
///creating DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//STRIPEEEEEEEEEEEEEEE
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//STRIPEEEEEEEEEEEEEEE



///THIS NEEDS EMAIL CONFIRMATION
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
///THIS doesnt NEED EMAIL CONFIRMATION
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

////THIS WILL ADD ROLE
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = $"/Identity/Account/Login";
    opt.LogoutPath = $"/Identity/Account/Logout";
    opt.AccessDeniedPath = $"/Identity/Account/AccessDenied";
}
);

///FACEBOOK
builder.Services.AddAuthentication().AddFacebook(opt =>
{
    opt.AppId = "2701098113378841";
    opt.AppSecret = "5c0a368436ce118a859579a93b649453";
});
///FACEBOOK
///


///creating DB the run on tools Nuget Package mngr console command "update-database"
///

///SESSION
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
///SESSION

//////Db Initializer for deployment
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
//////Db Initializer for deployment


/////this service is added to also use RAZOR PAGES on MVC
builder.Services.AddRazorPages();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

///Db Initializer for deployment
SeedDatabase();
///Db Initializer for deployment

///ADD RAZOR PAGES
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}