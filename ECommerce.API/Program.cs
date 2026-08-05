using Asp.Versioning;
using ECommerce.API;
using ECommerce.API.Endpoints;
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

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Products", policy =>
    {
        // Every query-string field the spec reads has to vary the key, otherwise
        // two different searches or sorts collide on the same cached page.
        policy.Expire(TimeSpan.FromMinutes(1))
        .SetVaryByQuery(
            "pageNumber", "pageSize", "search",
            "brandId", "typeId", "sortBy", "sortDescending");
    });

});

var app = builder.Build();

app.UseExceptionHandler();
app.UseOutputCache();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

app.MapProductEndpoints(apiVersionSet);
app.MapTypeEndpoints(apiVersionSet);
app.MapBrandEndpoints(apiVersionSet);
//app.MapBasketEndpoints(apiVersionSet);
//app.MapUserEndpoints(apiVersionSet);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // runs middleware to make the openapi doc available to use.
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });

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

app.Run();
