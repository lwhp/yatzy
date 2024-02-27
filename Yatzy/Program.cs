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

            string answer = Console.ReadLine() ?? "";

            if (answer != "ja" && answer != "nej")
            {
                Console.Clear();
                return ShouldReroll(cubeRolls);
            }

            return answer == "ja";
        }

        static void ChooseStarterPlayers()
        {
            Console.Write("Hvor mange spillere? (1-3): ");

            if (int.TryParse(Console.ReadLine(), out int value) && value > 0 && value < 4)
            {
                Console.Clear();
                for (int i = 0; i < value; i++)
                {
                    Console.Write("Indtast spiller " + (i + 1) + " navn: ");
                    Scoreboard.AddPlayer(Console.ReadLine() ?? "");
                }

                Console.WriteLine("Spillet starter om 3");
                Thread.Sleep(1000);
                Console.WriteLine("2");
                Thread.Sleep(1000);
                Console.WriteLine("1");
                Thread.Sleep(1000);
                Console.Clear();
            } else
            {
                Console.WriteLine("FEJL! Prøv igen");
                ChooseStarterPlayers();
            }
        }

        static void Main()
        {
            ChooseStarterPlayers();

            Die dice = new();

            // Et for loop for hver runde der afsluttes efter 15 runder
            string currentPlayer = Scoreboard.GetRandomStartPlayer();
            while (currentPlayer != "")
            {
                Scoreboard.PrintScoreboard();

                Console.WriteLine($"{currentPlayer} slår med terningerne");
                Console.WriteLine("Tryk enter for at slå med terningerne");
                Console.ReadLine();

                Console.Clear();

                List<sbyte> cubeRolls = dice.GetRandomRolls(5);

                for (int tur = 0; tur < 2; tur++)
                {
                    if (ShouldReroll(cubeRolls))
                        dice.StartRerolls(cubeRolls);
                    else
                        break;

                    Thread.Sleep(1000);
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

            Console.WriteLine("The game has concluded, and the winner is " + Scoreboard.GetWinner() + "!");
            Console.WriteLine("press enter to exit the program or type 'restart' to start a new game!");

            Scoreboard.PrintScoreboard();
            Console.SetCursorPosition(0, 3);

            if (Console.ReadLine() == "restart")
            {
                Console.Clear();
                Main();
            }
        }
    }
}
