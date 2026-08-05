using ECommerce.API;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
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

    var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityStoreDbContext>();

    // Both contexts share a database but keep separate migration histories,
    // so each one has to be migrated before any seeder touches it.
    await dbContext.Database.MigrateAsync();

    await identityDbContext.Database.MigrateAsync();

    await dbseed.SeedAll();
}

app.MapControllers();

app.Run();
