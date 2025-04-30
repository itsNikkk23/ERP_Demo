
using ERP.Repositories;
using ERP.Data;
using ERP.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<DbHelper>();
builder.Services.AddScoped<IEmployeeRepositories, EmployeeRepositories>();
builder.Services.AddScoped<IERPRepository, ERPRepository>();
builder.Services.AddScoped<ICustomersRepositories, CustomersRepositories>();
builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromHours(1); // Session expires after 1 hour of inactivity
    options.IdleTimeout = TimeSpan.FromDays(2); // 
    //options.IdleTimeout = TimeSpan.FromMinutes(1);


    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();

///


