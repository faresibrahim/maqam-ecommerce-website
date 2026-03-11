using MusicShoppingCartMvcUI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicShoppingCartMvcUI;
using MusicShoppingCartMvcUI.Data;
using MusicShoppingCartMvcUI.Models;
using MusicShoppingCartMvcUI.Shared;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    //IdentityRole represents roles like user, admin,etc. Identity user represents the user entity(id, username, email, passwordhash,etc.)
    //options.SignIn.RequireConfirmedAccount = true --> this forces the users to confirm their email beofre they can login
    .AddEntityFrameworkStores<ApplicationDbContext>()     //this line tells the identity to store users and roles inside this dbContext using EF Core       
    .AddDefaultUI() //This line adds built-In Razor Pages UI for --> register, login, logout, forgot password, reset password, email confirmation.
    .AddDefaultTokenProviders(); //Add token generators used for --> email confirmation, password reset, two-factor authentication

builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IHomeRepository, dbHomeRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUserOrderRepository, UserOrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();

//we only run this once and then comment it out --What is DbSeeder? 
//using( var scope= app.Services.CreateScope())
//{
//    await DbSeeder.SeedDefaultData(scope.ServiceProvider); //if we put this line out of this scope, it will give an error it have to be in a scope separately
//}

//await DbSeeder.SeedDefaultData(app.Services);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Landing}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
