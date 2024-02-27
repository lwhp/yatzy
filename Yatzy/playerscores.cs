namespace Yatzy
{
    public class PlayerScores
    {
        private Dictionary<string, int> Scores;

        public int currentTurn;
        public string name;

        public static int playerCount = 0;

        public bool HasPlayerScored(string rule)
        {
            return Scores[rule] != -1;
        }

        public int GetPlayerScore(string rule)
        {
            return Scores[rule];
        }

        public int GetPlayerTotalScore()
        {
            int score = 0;

            foreach (KeyValuePair<string, int> pair in Scores)
            {
                score += pair.Value;
            }

            return score;
        }

        private void ApplyBonus()
        {
            int score = 0;

            foreach (KeyValuePair<string, int> pair in Scores)
            {
                string rule = pair.Key;

                if (rule == "enere" || rule == "toere" || rule == "trere" || rule == "fiere" || rule == "femere" || rule == "seksere")
                    score += pair.Value;
            }

            if (score > 63)
                Scores["bonus"] = 50;


            if (score > 93)
                Scores["extrabonus"] = 100;

        }

        public void SetPlayerScore(string rule, int score)
        {
            Scores[rule] = score;

            if (rule == "enere" || rule == "toere" || rule == "trere" || rule == "fiere" || rule == "femere" || rule == "seksere")
            {
                ApplyBonus();
            }
        }

        public bool CanPlayNextRound()
        {
            foreach (KeyValuePair<string, int> pair in Scores)
            {
                if (pair.Value == -1)
                    return true;
            }

            return false;
        }

        public PlayerScores(string playerName)
        {
            playerCount++;
            name = playerName;
            currentTurn = playerCount;
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
                {"house", -1},
                {"chance", -1},
                {"yatzy", -1}
            };
        }
    }

}
