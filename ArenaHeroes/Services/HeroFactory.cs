using ArenaHeroes.Models;
using Microsoft.Extensions.Options;

namespace ArenaHeroes.Services
{
    public class HeroFactory
    {
        private readonly IOptions<HeroesOptions> options;
        private int id = 0;

        public HeroFactory(IOptions<HeroesOptions> options)
        {
            this.options = options;
        }

        private string GetId(HeroKind kind)
            => $"{kind}_{++id:D5}";

        internal void Clear()
        {
            id = 0;
        }

        internal IHero Create(HeroKind kind)
            => kind switch
            {
                HeroKind.Archer => new Archer(GetId(kind), options.Value.ArcherMaxHealth),
                HeroKind.Rider => new Rider(GetId(kind), options.Value.RiderMaxHealth),
                HeroKind.SwordsMan => new SwordsMan(GetId(kind), options.Value.SwordsmanMaxHealth),
                _ => throw new ArgumentOutOfRangeException($"{kind}"),
            };
    }
}
