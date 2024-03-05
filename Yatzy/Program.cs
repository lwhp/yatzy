namespace Yatzy
{
    internal class Program
    {
        // A method to show the current dice rolls
        static void ShowRolls(List<sbyte> rolls)
        {
            string text = string.Join(", ", rolls);

            Console.WriteLine(text.TrimEnd(','));
        }

        // A method which returns true/false depending if the player wishes to reroll the current dice
        static bool ShouldReroll(List<sbyte> cubeRolls)
        {
            Console.Write("You rolled: ");
            ShowRolls(cubeRolls);
            Console.WriteLine("Do you wish to reroll?");
            Console.Write("Yes/No: ");

            string answer = (Console.ReadLine() ?? "").ToLower();

            if (answer != "yes" && answer != "no")
            {
                Console.Clear();
                return ShouldReroll(cubeRolls);
            }

            return answer == "yes";
        }

        // A method which is run once per game to determain the amount of players
        static void SelectPlayerCount()
        {
            Console.Write("How many players? (1-3): ");

            if (int.TryParse(Console.ReadLine(), out int value) && value > 0 && value < 4)
            {
                Console.Clear();
                for (int i = 0; i < value; i++)
                {
                    Console.Write("Player " + (i + 1) + "'s name: ");
                    Scoreboard.AddPlayer(Console.ReadLine() ?? "");
                }

                Console.WriteLine("The game starts in 3");
                Thread.Sleep(1000);
                Console.WriteLine("2");
                Thread.Sleep(1000);
                Console.WriteLine("1");
                Thread.Sleep(1000);
                Console.Clear();
            } else
            {
                Console.WriteLine("ERROR! Try again");
                SelectPlayerCount();
            }
        }

        static void Main()
        {
            SelectPlayerCount();

            // A variable which reflects our current player
            string currentPlayer = Scoreboard.GetRandomStartPlayer();

            // Our while loop continously run untill we no longer can find a player that needs missing scores
            while (currentPlayer != "")
            {
                Scoreboard.PrintScoreboard();

                Console.WriteLine($"{currentPlayer} rolls the dice");
                Console.WriteLine("Press ENTER to roll the dice");
                Console.ReadLine();

                Console.Clear();

                List<sbyte> cubeRolls = Die.GetRandomRolls(5);

                for (int tur = 0; tur < 2; tur++)
                {
                    if (ShouldReroll(cubeRolls))
                    {
                        Die.StartRerolls(cubeRolls);
                    }
                    else
                    {
                        break;
                    }

                    Thread.Sleep(1000);
                    Console.Clear();
                }

                Console.Clear();

                Console.Write("Your rolls: ");

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
