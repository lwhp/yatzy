namespace Yatzy
{
    // PlayerScores class, the reason we make this a normal class is because we want to make instances for each individual players
    public class PlayerScores
    {
        // Private dictionairy that holds our player scores, the reason we make it a private field is to make sure no changes outside this class is made
        private Dictionary<string, int> Scores;

        // Public variables accessible outside of our class
        public sbyte currentTurn;
        public string name;

        // Method which returns a true/false depending if the player has scored
        public bool HasPlayerScored(string rule)
        {
            return Scores[rule] != -1;
        }

        // Method which returns the scorecard of the rule
        public int GetPlayerScore(string rule)
        {
            return Scores[rule];
        }

        // Method which returns the players total score
        public int GetPlayerTotalScore()
        {
            int score = 0;

            // here we use the KeyValuePair to use proper type declarations
            foreach (KeyValuePair<string, int> pair in Scores)
            {
                score += pair.Value;
            }

            return score;
        }

        // Private method which applies bonus, the reason this is private is because we don't want to make it accessible outside our playerscores class
        private void ApplyBonus()
        {
            int score = 0;

            // here we use the KeyValuePair to use proper type declarations
            foreach (KeyValuePair<string, int> pair in Scores)
            {
                string rule = pair.Key;

                if (rule == "enere" || rule == "toere" || rule == "trere" || rule == "fiere" || rule == "femere" || rule == "seksere")
                {
                    score += pair.Value;
                }
            }

            if (score > 63)
            {
                Scores["bonus"] = 50;
            }

            if (score > 93)
            {
                Scores["extrabonus"] = 100;
            }
        }

        // A method which sets the player score depending on the rule
        public void SetPlayerScore(string rule, int score)
        {
            Scores[rule] = score;

            if (rule == "enere" || rule == "toere" || rule == "trere" || rule == "fiere" || rule == "femere" || rule == "seksere")
            {
                ApplyBonus();
            }
        }

        // A method which returns true/false depending if the player have any pending rules to play
        public bool CanPlayNextRound()
        {
            foreach (KeyValuePair<string, int> pair in Scores)
            {
                if (pair.Value == -1)
                    return true;
            }

            return false;
        }

        // Here we have our class constructor which have name and turn arguments, we use those arguments to furfill our public class variables
        public PlayerScores(string playerName, sbyte playerTurn)
        {
            name = playerName;
            currentTurn = playerTurn;
            Scores = new Dictionary<string, int>
            {
                {"seksere", -1},
                {"femere", -1},
                {"fiere", -1},
                {"trere", -1},
                {"toere", -1},
                {"enere", -1},
                {"bonus", -1},
                {"extrabonus", -1},
                {"paret", -1},
                {"parto", -1 },
                {"treens", -1 },
                {"fiereens", -1},
                {"lilstraight", -1},
                {"bigstraight", -1},
                {"chance", -1},
                {"yatzy", -1}
            };
        }
    }

}
