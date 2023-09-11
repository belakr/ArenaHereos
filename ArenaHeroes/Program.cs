using ArenaHeroes.Models;
using ArenaHeroes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

string[] arguments = Environment.GetCommandLineArgs();
if (arguments.Length < 2)
{
    Console.WriteLine($"usage: ArenaHeroes [count]");
    return;
}

if (!int.TryParse(arguments[1], out int count))
{
    Console.WriteLine($"invalid parameter: {arguments[1]}");
    return;
}

if (count < 2)
{
    Console.WriteLine($"count must be greater than 1, actual value: {count}");
    return;
}

AppDomain.CurrentDomain.ProcessExit += (s, e) => Environment.Exit(0);

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var services = new ServiceCollection();
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(o => 
    {
        o.SingleLine = true;
        o.IncludeScopes = true;
    });
    builder.AddLog4Net();
    builder.SetMinimumLevel(LogLevel.Debug);
});

services.AddLogging();
services.AddSingleton(loggerFactory);
services.AddSingleton(configuration);
services.AddSingleton<RandomService>();
services.AddSingleton<HeroFactory>();
services.AddSingleton<ArenaService>();
services.Configure<HeroesOptions>(configuration.GetSection("heroes"));

ServiceLocator.SetServiceCollection(services);

var logger = loggerFactory.CreateLogger(nameof(Program));
try
{
    var arena = ServiceLocator.GetRequiredService<ArenaService>();

    logger.LogInformation($"generating {count} heroes...");
    arena.GenerateHeroes(count);
    logger.LogInformation("fight start, there can be only one !!!");
    arena.Fight();
    logger.LogInformation("fight end");
}
catch (Exception e)
{
    logger.LogError(e.ToString());
}
