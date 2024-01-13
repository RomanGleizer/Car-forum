using AutoMapper;
using CarForum.Database;
using CarForum.Mapping;
using CarForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));

builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(mappingConfig.CreateMapper());
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection))
    .AddIdentity<User, IdentityRole>(options => {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
