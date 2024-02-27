using System;
using System.Threading;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace Yatzy
{
    internal class Program
    {
        static void ShowRolls(List<sbyte> rolls)
        {
            for (int i = 0; i < rolls.Count; i++)
            {
                if (i + 1 == rolls.Count)
                    Console.Write(rolls[i] + "\n");
                else
                    Console.Write(rolls[i] + ", ");
            }
        }


        static bool ShouldReroll(List<sbyte> cubeRolls)
        {
            Console.Write("Du smed med terningerne og fik: ");
            ShowRolls(cubeRolls);
            Console.WriteLine("Ønkser du at reroll nogle terninger?");
            Console.Write("Skriv ja/nej: ");

            string answer = Console.ReadLine();

            if (answer != "ja" && answer != "nej")
            {
                Console.Clear();
                return ShouldReroll(cubeRolls);
            }

            return answer == "ja";
        }

        static void Main()
        {
            Die dice = new Die();
            Scoreboard.AddPlayer("Laurids");

            // Et for loop for hver runde der afsluttes efter 15 runder
            string currentPlayer = Scoreboard.GetRandomStartPlayer();
            while (currentPlayer != "")
            {
                Scoreboard.PrintScoreboard();

                Console.WriteLine($"{currentPlayer} slår med terningerne");
                Console.Write("Tryk enter for at slå med terningerne");
                Console.ReadLine();

                Console.Clear();

                List<sbyte> cubeRolls = dice.GetRandomRolls(5);

                for (int tur = 0; tur < 2; tur++)
                {
                    if (ShouldReroll(cubeRolls))
                        dice.StartRerolls(cubeRolls);
                    else
                        break;

                    Thread.Sleep(1500);
                    Console.Clear();
                }

                Console.Clear();

                Console.Write("Dine terninger: ");

                ShowRolls(cubeRolls);

                Console.Write("\n");

                Scoreboard.PrintPlayerScoreboard(currentPlayer, cubeRolls);

                Console.Clear();

                currentPlayer = Scoreboard.GetNextPlayerName(currentPlayer);
            }

            string winner = Scoreboard.GetWinner();

            Console.WriteLine("The game has concluded, and the winner is " + winner + "!");



            Console.WriteLine("press enter to exit the program or type 'restart' to start a new game!");

            Scoreboard.PrintScoreboard();

            if (Console.ReadLine() == "restart")
                Main();
        }
    }
}
