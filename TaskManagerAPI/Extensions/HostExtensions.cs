using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Extensions
{
    public static class HostExtensions
    {
        public static async Task ApplyMigrationsAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}


