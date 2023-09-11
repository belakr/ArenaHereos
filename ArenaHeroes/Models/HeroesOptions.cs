using System.Text.Json.Serialization;

namespace ArenaHeroes.Models
{
    public class HeroesOptions
    {
        [JsonPropertyName("archerMaxHealth")]
        public int ArcherMaxHealth { get; set; } = 100;
        [JsonPropertyName("riderMaxHealth")]
        public int RiderMaxHealth { get; set; } = 150;
        [JsonPropertyName("swordsmanMaxHealth")]
        public int SwordsmanMaxHealth { get; set; } = 120;
    }
}
