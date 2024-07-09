using IVYModa.EF;
using IVYModa.Models;
using IVYModa.Services;
using IVYModa.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserValidator<CustomUserValidator<AppUser>>();

// Cau hinh Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    //options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    //options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // SignIn
    options.SignIn.RequireConfirmedEmail = true; // Xac thuc email
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/customer/login";
    options.LogoutPath = "/customer/logout";
    options.AccessDeniedPath = "/";
});

// AddTransient: Dich vu duoc tao moi khi no duoc yeu cau.
// AddScoped: Dich vu duoc tao mot lan cho moi yeu cau HTTP.
// AddSingleton: Dich vu duoc tao mot lan duy nhat.
builder.Services.AddScoped<SendMailService>();
//builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();

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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
