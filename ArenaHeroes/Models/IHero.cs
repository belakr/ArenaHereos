using ArenaHeroes.Services;

namespace ArenaHeroes.Models
{
    public enum HeroKind
    {
        Archer = 0,
        Rider = 1,
        SwordsMan = 2
    }

    public interface IHero
    {
        HeroKind Kind { get; }
        string Id { get; }
        int Health { get; }
        int MaxHealth { get; }

        bool IsAlive();
        void Relax();
        void Attack(IHero defender);
        void Died();
    }

    internal abstract class Hero
    {
        public Hero(string id, int maxHealth)
        {
            Id = id;
            MaxHealth = maxHealth;
            Health = MaxHealth;
        }

        public string Id { get; }
        public int Health { get; private set;}
        public int MaxHealth { get; }
        public bool IsAlive() => Health > 0;

        public void Relax()
        {
            Health += 10;
            Health = Math.Min(MaxHealth, Health);
        }

        public void Died()
        {
            Health = 0;
        }

        private void Fight()
        {
            if (!IsAlive()) return;
            Health /= 2;
            if (Health < MaxHealth / 4)
                Health = 0;
        }

        public virtual void Attack(IHero defender)
        {
            Fight();
            (defender as Hero)?.Fight();
        }

        public override string ToString()
            => $"{Id}, {nameof(Health)}: {Health}, IsAlive: {IsAlive()}";
    }

    internal class Archer : Hero, IHero
    {
        public Archer(string id, int maxHealth) : base(id, maxHealth) { }
        public HeroKind Kind => HeroKind.Archer;

        public override void Attack(IHero defender)
        {
            switch (defender.Kind)
            {
                case HeroKind.SwordsMan:
                case HeroKind.Archer:
                    defender.Died();
                    break;
                case HeroKind.Rider:
                    var rnd = ServiceLocator.GetRequiredService<RandomService>().Generate(5);
                    if (rnd < 2) defender.Died();  // 40%
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{defender.Kind}");
            }
            base.Attack(defender);
        }
    }

    internal class Rider : Hero, IHero
    {
        public Rider(string id, int maxHealth) : base(id, maxHealth) { }
        public HeroKind Kind => HeroKind.Rider;

        public override void Attack(IHero defender)
        {
            switch (defender.Kind)
            {
                case HeroKind.Archer:
                case HeroKind.Rider:
                    defender.Died();
                    break;
                case HeroKind.SwordsMan:
                    Died();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{defender.Kind}");
            }
            base.Attack(defender);
        }
    }

    internal class SwordsMan : Hero, IHero 
    {
        public SwordsMan(string id, int maxHealth) : base(id, maxHealth) { }
        public HeroKind Kind => HeroKind.SwordsMan;

        public override void Attack(IHero defender)
        {
            switch (defender.Kind)
            {
                case HeroKind.Rider: // nothing
                    break;
                case HeroKind.SwordsMan:
                case HeroKind.Archer:
                    defender.Died();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{defender.Kind}");
            }
            base.Attack(defender);
        }
    }
}
