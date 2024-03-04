using System.Linq;

namespace Yatzy
{

    public class Die
    {
        readonly Random rand = new();

        // Generate random amounts of roles
        public List<sbyte> GetRandomRolls(int amount)
        {
            List<sbyte> rolls = [];

            for (int i = 0; i < amount; i++)
            {
                rolls.Add((sbyte)rand.Next(1, 7));
            }

            return rolls;
        }

        private static int[] GetRerollRoles(string rerolls)
        {
            // Fjern alt whitespace fra bruger input 1, 2, 3, 4 -> 1,2,3,4
            string removeSpace = rerolls.Replace(" ", "");

            // Splitter vi vores string til et array 1,2,3,4 -> [1, 2, 3, 4]
            string[] stringArray = removeSpace.Split(",");

            // her laver vi et tomt array der ser sådan her ud []
            List<int> roles = new List<int>();

            for (int i = 0; i < stringArray.Length; i++)
            {

                if (int.TryParse(stringArray[i], out int value) && value < 6 && value > 0)
                {
                    roles.Add(value);
                }
            }



            return roles.ToArray();
        }

        public void StartRerolls(List<sbyte> dice)
        {
            Console.Write("Skriv terning tallene du gerne vil reroll f.eks. (1, 3, 5) ");

            int[] rerollArray = GetRerollRoles(Console.ReadLine() ?? "");

            foreach (int i in rerollArray)
            {
                int index = i - 1;

                if (dice.Count >= index)
                {
                    Console.WriteLine($"Reroller terning {i}");
                    dice[index] = GetRandomRolls(1)[0];

                    Thread.Sleep(500);

                    Console.WriteLine($"Du slog en {dice[index]}");
                    Thread.Sleep(500);
                }
            }
        }
    }
}
