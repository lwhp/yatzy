using System;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;

namespace Yatzy
{
    public static class Scoreboard
    {

        private static Dictionary<string, PlayerScores> Players = new Dictionary<string, PlayerScores>();

        public static void AddPlayer(string playerName)
        {
            Players[playerName] = new PlayerScores(playerName);
        }

        public static string GetNextPlayerName(string currentPlayer) {
            int currentTurn = Players[currentPlayer].currentTurn;
            int nextTurn = currentTurn == PlayerScores.playerCount ? 1 : currentTurn + 1;

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                if (player.Value.currentTurn == nextTurn && player.Value.CanPlayNextRound())
                    return player.Value.name;
            }

            // This is our fallback return, if no players can play (the entire game has concluded) then we return this
            return "";
        }

        public static string GetRandomStartPlayer()
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                list.Add(player.Key);
            }

            Random random = new Random();

            return list[random.Next(0, list.Count)];
        }

        public static Dictionary<string, PlayerScores> GetAllPlayers()
        {
            return Players;
        }

        public static bool AddPlayerPoints(string playerName, List<sbyte> rolls, string rule, sbyte sumOfDice)
        {
            PlayerScores player = Players[playerName];

            if (player.HasPlayerScored(rule))
            {
                Console.WriteLine("Du har allerede sat en værdi her!");
                return false;
            }

            if (Rules.simpleRules.ContainsKey(rule))
                player.SetPlayerScore(rule, Rules.GetSumOfSimple(rolls, rule));
            else
                player.SetPlayerScore(rule, Rules.GetSumOfAdvanced(rolls, sumOfDice, rule == "yatzy"));

            return true;
        }

        private readonly static Dictionary<string, string> ValueToString = new Dictionary<string, string>()
        {
            {"seksere", "6'ere"},
            {"femere", "5'ere"},
            {"fiere", "4'ere"},
            {"trere", "3'ere"},
            {"toere", "2'ere"},
            {"enere", "1'ere"},
            {"bonus", "Bonus" },
            {"paret", "Et Par" },
            {"parto", "To Par" },
            {"treens", "Tre Ens" },
            {"fiereens", "Fire Ens" },
            {"lilstraight", "Little Straight" },
            {"bigstraight", "Big Straight" },
            {"house", "House" },
            {"chance", "Chance" },
            {"yatzy", "YATZY!" }
        };

        private readonly static List<string> LoadOrder = ["enere", "toere", "trere", "fiere", "femere", "seksere", "bonus", "paret", "parto", "treens", "fiereens", "lilstraight", "bigstraight", "house", "chance", "yatzy"];
        public static void PrintScoreboard()
        {
            int playerIndex = 0;

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                // Her calculere vi distances mellem de forskellige spiller kort
                int xShift = playerIndex * 32;

                // Her fortæller vi programmet hvilken linje vi skal skrive noget tekst ud på
                sbyte yShift = 10;

                // Her bruger vi vores variabler til at shifte positionen af tekst
                Console.SetCursorPosition(xShift, yShift);

                // her udskriver vi vores spillers navne
                Console.WriteLine($"{player.Key, -23} Points");

                // Make sure our y axis has shiftet 1 to make room for new text
                yShift++;
                Console.SetCursorPosition(xShift, yShift);

                // Gem totalscoren på vores nuværende spiller
                int totalScore = 0;


                /* 
                 * Her udskriver vi basale informationer omkring den nuværendes spiller
                 * [RULE] - Points
                 * Rule = The current rule showcasing for the player
                 * Points = How many points the player has yielded for that specific rule
                 */
                foreach (string order in LoadOrder)
                {
                    string localeString = ValueToString[order];
                    int score = player.Value.Scores[order] < 0 ? 0 : player.Value.Scores[order];

                    // We need to use manual space padding as \t had issues with different lengths of strings
                    sbyte paddingRequired = (sbyte) Math.Max(0, 17 - localeString.Length);
                    string paddedLocaleString = $"[{localeString}]{new string(' ', paddingRequired)}";

                    Console.Write($"{paddedLocaleString} {score, 5}");

                    totalScore += score;

                    // after each rule we need to sshift our y by 1 so it prints on the next line
                    yShift++;
                    Console.SetCursorPosition(xShift, yShift);
                }

                // Write out our total score for the players to instantly know who's currently in the lead
                Console.Write($"[Total Score] {totalScore, 11}");

                playerIndex++;
            }

            Console.SetCursorPosition(0, 0);
        }


        public static string GetWinner()
        {
            string currentPlayer = "";
            int currentHighScore = 0;
            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                int score = 0;

                foreach (KeyValuePair<string, int> playerScore in player.Value.Scores)
                {
                    score += playerScore.Value;
                }

                if (score > currentHighScore)
                {
                    currentHighScore = score;
                    currentPlayer = player.Key;
                }
            }

            return currentPlayer;
        }

        public static sbyte GetZeroToFifteen()
        {
            while (!sbyte.TryParse(Console.ReadLine(), out sbyte value) || value >= 0 && value <= 15)
            {
                return value;
            }

            return 0;
        }

        public static void PrintPlayerScoreboard(string currentPlayer, List<sbyte> rolls)
        {
            PlayerScores player = Players[currentPlayer];


            int yShift = 10;

            Console.SetCursorPosition(0, yShift);
            Console.WriteLine($"{"", -31} Points");

            for (int i = 0; i < LoadOrder.Count; i++)
            {
                string order = LoadOrder[i];
                yShift++;
                Console.SetCursorPosition(0, yShift);

                string LocaleString = ValueToString[order];
                int score = player.Scores[order] < 0 ? 0 : player.Scores[order];

                Console.Write($"{i, -10} {LocaleString, -20} {score, -10}");
            }

            Console.SetCursorPosition(0, 0);

            string rule;
            sbyte sum;
            do 
            {
                Console.Clear();
                Console.WriteLine($"Hvor vil du krydse af? (0 - {LoadOrder.Count - 1})");

                sbyte value = GetZeroToFifteen();

                rule = LoadOrder[value];

                switch (rule)
                {
                    case "paret":
                        sum = 2;
                        break;
                    case "parto":
                        sum = 4;
                        break;
                    case "treens":
                        sum = 3;
                        break;
                    case "fiereens":
                        sum = 4;
                        break;
                    case "yatzy":
                        sum = 5;
                        break;
                    default:
                        sum = 0;
                        break;
                };
            }
            while (!AddPlayerPoints(currentPlayer, rolls, rule, sum));
        }
    }
}
