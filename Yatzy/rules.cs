namespace Yatzy
{
   /*
    * Rule static class, here we hold all of our logic of giving points per rule
    * The reason this class is static is because we don't want to make instances but just use the methods
    */

    public static class Rules
    {
        // A dictionairy which holds all of our "simpleRules" simply rules are defined by needing specific to give points.
        // If the length of the ruleset is 6 that means its a strict rule and only give the points if they have all the dice
        public static readonly Dictionary<string, sbyte[]> simpleRules = new()
        {
            { "enere", [1, 1, 1, 1, 1] },
            { "toere", [2, 2, 2, 2, 2] },
            { "trere", [3, 3, 3, 3, 3] },
            { "fiere", [4, 4, 4, 4, 4] },
            { "femere", [5, 5, 5, 5, 5] },
            { "seksere", [6, 6, 6, 6, 6] },
            { "lilstraight", [1, 2, 3, 4, 5, 15] },
            { "bigstraight", [2, 3, 4, 5, 6, 20] },
        };

        // Get the sum of our simple ruleset and return points
        public static int GetSumOfSimple(List<sbyte> dice, string rule)
        {
            sbyte[] currentRules = simpleRules[rule];

            bool isStrict = currentRules.Length > 5;

            int points = 0;

            for (int i = 0; i < 5; i++)
            {
                sbyte die = currentRules[i];

                if (dice.Contains(die))
                {
                    points += die;
                    dice.Remove(die);
                }
                else if (isStrict)
                {
                    return 0;
                }
            }

            return points;
        }

        // Using lambda expression we can get the sum of the dice, by looping over it
        public static sbyte GetSumOfDice(List<sbyte> dice) => (sbyte)dice.Sum(x => x);

        // Return the points of the advanced ruleset (used for numberofkind, pairs and yahtzy)
        public static int GetSumOfAdvanced(List<sbyte> dice, sbyte requireDicecount, bool isYahtzy)
        {
            // Key is the die face and value is the amount of dice with that die face
            Dictionary<sbyte, sbyte> counts = [];

            foreach (sbyte die in dice)
            {
                if (!counts.ContainsKey(die))
                    counts[die] = 1;
                else
                    counts[die]++;
            }

            int score = 0;

            foreach (KeyValuePair<sbyte, sbyte> pair in counts)
            {
                int newScore = requireDicecount * pair.Key;
                if (pair.Value >= requireDicecount && newScore > score)
                {
                    score = isYahtzy ? 50 : newScore;
                }
            }

            return score;
        }
    }
}
