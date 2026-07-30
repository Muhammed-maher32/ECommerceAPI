using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public static class JsonSeeder
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedIfEmpty<TEntity, TModel>(
        DbSet<TEntity> dbset,
        string fileName,
        Func<TModel, Result<TEntity>> map,
        CancellationToken ct = default
        ) where TEntity : BaseEntity
    {
        if (await dbset.AnyAsync(ct))
            return;

        var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data", fileName);

        if (!File.Exists(filePath)) return;

        await using var stream = File.OpenRead(filePath);

        var models = await JsonSerializer.DeserializeAsync<List<TModel>>(stream, options, ct);

        if (models is null || models.Count == 0) return;

        var entities = new List<TEntity>();

        foreach (var model in models)
        {
            var result = map(model);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                     $"Invalid record '{model}' in '{fileName}'. Error: {result.Error!.Message}");
            }

            entities.Add(result.Value);
        }

        await dbset.AddRangeAsync(entities, ct);
    }
}
