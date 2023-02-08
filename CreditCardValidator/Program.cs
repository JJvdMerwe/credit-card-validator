using Application.Common.Interfaces;
using Application.DataSeeder;
using Domain.Entities;
using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EFContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var context = new EFContext())
{
    context.Database.EnsureCreated();

    var anyCreditCardProviders = await context.Set<CreditCardProvider>().AnyAsync();

    if (!anyCreditCardProviders)
    {
        var creditCardProviders = context.Set<CreditCardProvider>();

        creditCardProviders.Add(new CreditCardProvider() { Name = "VISA", CardNumberRegEx = "^4[0-9]{12}(?:[0-9]{3})?$", DateCreated = DateTime.Now });
        creditCardProviders.Add(new CreditCardProvider() { Name = "Mastercard", CardNumberRegEx = "^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$", DateCreated = DateTime.Now });
        creditCardProviders.Add(new CreditCardProvider() { Name = "Amex", CardNumberRegEx = "^3[47][0-9]{13}$", DateCreated = DateTime.Now });
        creditCardProviders.Add(new CreditCardProvider() { Name = "Discover", CardNumberRegEx = "^6(?:011|5[0-9]{2})[0-9]{12}$", DateCreated = DateTime.Now });

        context.SaveChanges();
    }
}

app.Run();
