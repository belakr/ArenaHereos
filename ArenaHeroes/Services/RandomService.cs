namespace ArenaHeroes.Services
{
    public class RandomService
    {
        private readonly Random random = new();

        public int Generate(int maxValue, int minValue = 0)
            => random.Next(minValue, maxValue);

        public int GenerateExcept(int maxValue, int except, int minValue = 0)
        {
            int result;
            do
            {
                result = Generate(maxValue, minValue);
            } while (result == except);
            return result;
        }
    }
}
