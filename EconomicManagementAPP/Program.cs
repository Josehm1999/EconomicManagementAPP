using EconomicManagementAPP.Services;
using EconomicManagementAPP.Repositories;
using Microsoft.AspNetCore.Identity;
using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authenticatedUsersPolicy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(authenticatedUsersPolicy));
});

builder.Services.AddTransient<IRepositorieAccountTypes, RepositorieAccountTypes>();
builder.Services.AddTransient<IRepositorieUsers, RepositorieUser>();
builder.Services.AddTransient<IRepositorieCategories, RepositorieCategories>();
builder.Services.AddTransient<IRepositorieAccounts, RepositorieAccounts>();
builder.Services.AddTransient<IRepositorieTransactions, RepositorieTransactions>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IRepositorieCategories, RepositorieCategories>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IUserStore<Users>, UserStore>();
builder.Services.AddTransient<SignInManager<Users>>();
builder.Services.AddIdentityCore<Users>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
    }).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/users/login";
});


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

app.Run();
