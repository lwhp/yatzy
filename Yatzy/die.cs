using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int[] rolls = new int[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                rolls[i] = int.Parse(stringArray[i]);
            }

            return rolls;
        }

        public void StartRerolls(List<sbyte> dice)
        {
            Console.Write("Skriv terning tallene du gerne vil reroll f.eks. (1, 3, 5) ");

            #pragma warning disable CS8604 // Possible null reference argument.
            int[] rerollArray = GetRerollRoles(Console.ReadLine());
            #pragma warning restore CS8604 // Possible null reference argument.

            foreach (int i in rerollArray)
            {
                int index = i - 1;
                Console.WriteLine($"Reroller terning {i}");
                dice[index] = GetRandomRolls(1)[0];

                Thread.Sleep(500);

                Console.WriteLine($"Du slog en {dice[index]}");
                Thread.Sleep(500);
            }
        }
    }
}
