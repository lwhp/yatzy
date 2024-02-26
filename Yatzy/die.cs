using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{

    public class Die
    {
        Random rand = new Random();

        // Generate random amounts of roles
        public List<sbyte> GetRandomRolls(int amount)
        {
            List<sbyte> rolls = new List<sbyte>();

            for (int i = 0; i < amount; i++)
            {
                rolls.Add((sbyte)rand.Next(1, 7));
            }

            return rolls;
        }

        private int[] GetRerollRoles(string rerolls)
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

            int[] rerollArray = GetRerollRoles(Console.ReadLine());

            foreach (int i in rerollArray)
            {
                int index = i - 1;
                Console.WriteLine($"Reroller terning {i}");
                dice[index] = GetRandomRolls(1)[0];

                Thread.Sleep(750);

                Console.WriteLine($"Du slog en {dice[index]}");
            }
        }
    }
}
