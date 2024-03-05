namespace Yatzy
{
    /*
     * Die static class, here we hold most of our rolling logic which is either rolling or rerolling a die/dices
     * The reason this is a static class is due to the fact we do not want to make instances and just have a general class that has usefull methods
     */

    public static class Die
    {
        // we instance our random class once, as a readonly as we never want to write anything here
        static readonly Random rand = new();

        // The purpose of this method is to get an amount of random rolls
        public static List<sbyte> GetRandomRolls(sbyte amount)
        {
            // Make an empty list for our rolls
            List<sbyte> rolls = [];

            for (int i = 0; i < amount; i++)
            {
                // Add our dice roll with the explicit type sbyte
                rolls.Add((sbyte)rand.Next(1, 7));
            }

            // return the list of rolls
            return rolls;
        }

        // The purpose of this method is to send a string of rerolls from the user that could look like 1, 2, 3, 4, 5 and convert it into a int[]
        private static int[] GetRerollRoles(string rerolls)
        {
            // Remove all whitespace from user input 1, 2, 3, 4 -> 1,2,3,4
            string removeSpace = rerolls.Replace(" ", "");

            // Split the string into a array whereever there's a comma 1,2,3,4 -> [1, 2, 3, 4]
            string[] stringArray = removeSpace.Split(",");

            // Here we make an empty list of our rolls
            List<int> rolls = [];

            for (int i = 0; i < stringArray.Length; i++)
            {
                // Loop through our List to verify user input and make sure the value is above 0 and less than 6 (1-5)
                if (int.TryParse(stringArray[i], out int value) && value < 6 && value > 0)
                {
                    // Add the roll to our list
                    rolls.Add(value);
                }
            }

            // We convert our list into an array we send back
            return rolls.ToArray();
        }


        // The purpose of this method is to get and display the random rolls the player recieved
        public static void StartRerolls(List<sbyte> dice)
        {
            Console.Write("Write the die you want to reroll Ex. (1, 3, 5) ");

            // Get a int[] from calling GetRerollRoles with user input
            int[] rerollArray = GetRerollRoles(Console.ReadLine() ?? "");

            foreach (int i in rerollArray)
            {
                // we need to i - 1 here as our user input is formatted as 1-5 but our index ranges from 0-4
                int index = i - 1;

                // We have an if statement to make sure our index is NOT out of bounds
                if (dice.Count >= index)
                {
                    // Rerolls the dice
                    Console.WriteLine($"Rerolling die {i}");
                    dice[index] = GetRandomRolls(1)[0];

                    // Wait 500ms
                    Thread.Sleep(500);

                    // Showcase the roll and wait an additional 500ms
                    Console.WriteLine($"You rolled a {dice[index]}");
                    Thread.Sleep(500);
                }
            }
        }
    }
}
