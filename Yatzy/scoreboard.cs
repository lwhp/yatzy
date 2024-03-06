namespace Yatzy
{
    public static class Scoreboard
    {
        // Private Dictionary with all of our players and their objects, each player is stored by their name as the key
        private static Dictionary<string, PlayerScores> Players = [];

        // Private variable that stores the max players in the current game
        private static sbyte maxPlayers = 0;

        // Method to add a player object
        public static void AddPlayer(string playerName)
        {
            maxPlayers++;
            Players[playerName] = new PlayerScores(playerName, maxPlayers);
        }

        // A method whcih returns the next players name
        public static string GetNextPlayerName(string currentPlayer) {
            int currentTurn = Players[currentPlayer].currentTurn;
            int nextTurn = currentTurn == maxPlayers ? 1 : currentTurn + 1;

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                if (player.Value.currentTurn == nextTurn && player.Value.CanPlayNextRound())
                    return player.Value.name;
            }

            // This is our fallback return, if no players can play (the entire game has concluded) then we return empty a string
            return "";
        }

        // A method which determains a random starting player
        public static string GetRandomStartPlayer()
        {
            List<string> list = [];

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                list.Add(player.Key);
            }

            Random random = new();

            return list[random.Next(0, list.Count)];
        }

        // A method whcih adds the players points (returns true if points were successfully added or false if its already been set)
        public static bool AddPlayerPoints(string playerName, List<sbyte> rolls, string rule)
        {
            PlayerScores player = Players[playerName];

            if (player.HasPlayerScored(rule))
            {
                Console.WriteLine("Du har allerede sat en værdi her!");
                return false;
            }

            if (Rules.simpleRules.ContainsKey(rule))
                player.SetPlayerScore(rule, Rules.GetSumOfSimple(rolls, rule));
            else if (rule == "chance")
                player.SetPlayerScore(rule, Rules.GetSumOfDice(rolls));
            else
                player.SetPlayerScore(rule, Rules.GetSumOfAdvanced(rolls, GetRuleSum(rule), rule == "yatzy"));

            return true;
        }

        // A static dictionary which is used to translate Key values to readable strings for our users
        private readonly static Dictionary<string, string> ValueToString = new()
        {
            {"seksere", "Sixes"},
            {"femere", "Fives"},
            {"fiere", "Fours"},
            {"trere", "Threes"},
            {"toere", "Twos"},
            {"enere", "Aces"},
            {"bonus", "Bonus" },
            {"extrabonus", "Extra Bonus" },
            {"paret", "One Pair" },
            {"parto", "Two Pairs" },
            {"treens", "3 of Kind" },
            {"fiereens", "4 of Kind" },
            {"lilstraight", "Little Straight" },
            {"bigstraight", "Big Straight" },
            {"chance", "Chance" },
            {"yatzy", "YATZY!" }
        };

        // A private list which is here to determain loadorder and which values to show first
        private readonly static List<string> LoadOrder = ["enere", "toere", "trere", "fiere", "femere", "seksere", "bonus", "extrabonus", "paret", "parto", "treens", "fiereens", "lilstraight", "bigstraight", "chance", "yatzy"];
       
        // A method used to write out our entire player scoreboard
        public static void PrintScoreboard()
        {
            int playerIndex = 0;

            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                // Here we calculate our x shift based on the playerIndex (1-3) that we are currently writing out
                int xShift = playerIndex * 32;

                // We tell program which y line to start on
                sbyte yShift = 10;

                // We use our y and x variables to shift our current writing position
                Console.SetCursorPosition(xShift, yShift);

                // her udskriver vi vores spillers navne
                Console.WriteLine($"{player.Key, -23} Points");

                // Make sure our y axis has shiftet 1 to make room for new text
                yShift++;
                Console.SetCursorPosition(xShift, yShift);

                // Save our total score as an variable
                int totalScore = 0;

                /* 
                 * Here we write out the points and rules of each player.
                 * [RULE] - Points
                 * Rule = The current rule showcasing for the player
                 * Points = How many points the player has yielded for that specific rule
                 */
                foreach (string order in LoadOrder)
                {
                    string localeString = ValueToString[order];
                    int score = player.Value.GetPlayerScore(order) < 0 ? 0 : player.Value.GetPlayerScore(order);

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


        // A method to determain the winner of the the game
        public static string GetWinner()
        {
            string currentPlayer = "";
            int currentHighScore = 0;
            foreach (KeyValuePair<string, PlayerScores> player in Players)
            {
                int score = player.Value.GetPlayerTotalScore();

                if (score > currentHighScore)
                {
                    currentHighScore = score;
                    currentPlayer = player.Key;
                }
            }

            return currentPlayer;
        }

        // A private method to get a valid index number for our ruleset
        private static sbyte GetValidNumber(sbyte maxRoll)
        {
            if (sbyte.TryParse(Console.ReadLine() ?? "", out sbyte i))
            {
                if (i >= 0 && i <= maxRoll)
                {
                    return i;
                }
            }

            return GetValidNumber(maxRoll);
        }

        // A method which returns the number of kinds for our ruleset (used for our advanced rules)
        private static sbyte GetRuleSum(string rule) => rule switch
        {
            "paret" => 2,
            "parto" => 4,
            "treens" => 3,
            "fiereens" => 4,
            "yatzy" => 5,
            _ => 0,
        };

        // A method which write out the specified players scoreboard, this one also differs as it shows which fields the player can input into
        public static void PrintPlayerScoreboard(string currentPlayer, List<sbyte> rolls)
        {
            PlayerScores player = Players[currentPlayer];

            int yShift = 10;

            Console.SetCursorPosition(0, yShift);

            Console.WriteLine($"{"Number", -10} {"Rule",-15} Points");

            for (int i = 0; i < LoadOrder.Count; i++)
            {
                string order = LoadOrder[i];
                yShift++;
                Console.SetCursorPosition(0, yShift);

                string LocaleString = ValueToString[order];

                int score = player.GetPlayerScore(order) < 0 ? 0 : player.GetPlayerScore(order);

                Console.Write($"{i, -10} {LocaleString, -20} {score, -10}");
            }

            Console.SetCursorPosition(0, 1);

            string rule;
            sbyte maxRoll = (sbyte)(LoadOrder.Count - 1);
            do 
            {
                Console.WriteLine($"Where do you want to score? (0 - {maxRoll})");

                sbyte value = GetValidNumber(maxRoll);

                rule = LoadOrder[value];
            }
            while (!AddPlayerPoints(currentPlayer, rolls, rule));
        }
    }
}
