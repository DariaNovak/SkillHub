using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Common
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory>
    {
        protected readonly ApplicationDbContext Context;
        protected readonly HttpClient Client;
        private readonly IServiceScope _scope;

        protected BaseIntegrationTest(IntegrationTestWebFactory factory)
        {
            // Створюємо scope для доступу до сервісів
            _scope = factory.Services.CreateScope();

            // Отримуємо DbContext
            Context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Створюємо HTTP клієнт
            Client = factory.CreateClient();

            // Налаштовуємо автентифікацію
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");
        }

        protected async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();
        }

        protected async Task<T?> FindAsync<T>(params object[] keyValues) where T : class
        {
            return await Context.Set<T>().FindAsync(keyValues);
        }
    }
}
