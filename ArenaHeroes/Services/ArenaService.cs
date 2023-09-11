using ArenaHeroes.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ArenaHeroes.Services
{
    public class ArenaService
    {
        private readonly ILogger logger;
        private readonly RandomService randomService;
        private readonly HeroFactory heroFactory;
        private readonly List<IHero> heroes;

        public ArenaService(ILoggerFactory loggerFactory,
            RandomService randomService,
            HeroFactory heroFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(ArenaService));
            this.randomService = randomService;
            this.heroFactory = heroFactory;
            heroes = new List<IHero>();
        }

        internal void GenerateHeroes(int count)
        {
            heroes.Clear();
            heroFactory.Clear();
            heroes.AddRange(Enumerable.Range(0, count).Select(i => heroFactory.Create((HeroKind)randomService.Generate(3))));

            for (int i = 0; i < heroes.Count; i++)
                logger.LogInformation($"{(i + 1):D5} --  {heroes[i]}");

            var group = heroes.GroupBy(k => k.Kind);
            foreach (var g in group)
                logger.LogInformation($"{g.Key} count: {g.Count()}");
        }

        internal void Fight()
        {
            var aliveHeroes = heroes.Where(h => h.IsAlive()).ToList();
            var round = 0;
            while (aliveHeroes.Count > 1)
            {
                var attackerIndex = randomService.Generate(aliveHeroes.Count);
                var defenderIndex = randomService.GenerateExcept(aliveHeroes.Count, attackerIndex);
                var attacker = aliveHeroes[attackerIndex];
                var defender = aliveHeroes[defenderIndex];

                for (int i = 0; i < aliveHeroes.Count; i++)
                {
                    if (i == attackerIndex || i == defenderIndex) continue;
                    aliveHeroes[i].Relax();
                }

                logger.LogInformation($"Round {++round}, Attacker: {attacker.Id} {attacker.Health}, Defender: {defender.Id} {defender.Health}");
                attacker.Attack(defender);
                logger.LogInformation($"Round {round} result: Attacker: {attacker}, Defender: {defender}");
                aliveHeroes = heroes.Where(h => h.IsAlive()).ToList();
                logger.LogDebug($"Still alive: {aliveHeroes.Count}");
            }

            var winner = aliveHeroes.SingleOrDefault();
            if (winner != null)
                logger.LogInformation($"The winner is: {winner}");
            else
                logger.LogInformation("There isn't winner");

        }
    }
}
