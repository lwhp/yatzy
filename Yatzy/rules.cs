using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
    public static class Rules
    {
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
            { "house", [2, 2, 5, 5, 5, 50] },
        };

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
                    return 0;
            }

            return points;
        }


        public static int GetSumOfAdvanced(List<sbyte> dice, sbyte numberOfKind, bool isYahtzy)
        {
            sbyte score = 0;

            // her lavet vi en dictionairy, dette differentiere sig fra et "array" or en "list" da man kan sætter index til at være et unikt key, istedet for at være 0-1-2-3-4 osv.
            Dictionary<sbyte, sbyte> counts = [];

            foreach (sbyte die in dice)
            {
                if (!counts.TryGetValue(die, out sbyte value))
                    counts[die] = 1;
                else
                    counts[die] = value++;

                score += die;
            }

            foreach (sbyte amountOfKind in counts.Values)
            {
                if (amountOfKind >= numberOfKind)
                    return isYahtzy ? 50 : score;
            }

            return 0;
        }
    }
}
