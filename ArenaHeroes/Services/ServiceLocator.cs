using Microsoft.Extensions.DependencyInjection;

namespace ArenaHeroes.Services
{
    public static class ServiceLocator
    {
        private static ServiceCollection? services;
        private static ServiceProvider? serviceProvider;

        public static void SetServiceCollection(ServiceCollection _services)
        {
            services = _services;
            serviceProvider = services.BuildServiceProvider();
        }

        public static bool IsSet => Current != null;
        public static IServiceProvider? Current => serviceProvider;
        public static TService? GetService<TService>() where TService : notnull => Current == null ? default : Current.GetService<TService>();
        public static TService GetRequiredService<TService>() where TService : notnull => Current != null ? Current.GetRequiredService<TService>() : throw new ArgumentNullException(nameof(Current));
        public static bool IsRegistered<TService>() where TService : notnull => services?.Any(x => x.ServiceType == typeof(TService)) == true;
    }
}
