using MusicIndustries.ProductLoader.Data.Context;
using MusicIndustries.ProductLoader.Data.MigrationHelpers;
using ProductLoader.Web.Configuration;
using ProductLoader.Web.HostedServices;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Loader Core Dependencies
builder.Services.AddLoaderWebDependencies();
builder.Services.AddElseWorkflow();

// Register the hosted service
builder.Services.AddHostedService<PublisherHost>();
builder.Services.AddHostedService<SubscriberHost>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.AddLoaderWebDependencies();
app.Run();
