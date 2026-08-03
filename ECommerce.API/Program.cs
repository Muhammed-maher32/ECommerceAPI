using ECommerce.API;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // runs middleware to make the openapi doc available to use.
    app.UseSwaggerUI();

    await using var scope = app.Services.CreateAsyncScope();

    var dbseed = scope.ServiceProvider.GetRequiredService<DataBaseSeeder>();

    var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

    await dbContext.Database.MigrateAsync();

    await dbseed.SeedAll();
}

app.MapControllers();

app.Run();
