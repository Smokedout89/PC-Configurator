namespace PCConfigurator
{
    using System.Text.Json;
    using Models;
    using Utilities;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Read data from json file and deserialize it.
            var jsonData = File.ReadAllText("../../../JsonData/pc-store-inventory.json");
            Configuration configuration = JsonSerializer.Deserialize<Configuration>(jsonData);


            Console.WriteLine(UserMessages.WelcomeMessage);
            Console.WriteLine(UserMessages.UserChoices);

            string userChoice;

            while ((userChoice = Console.ReadLine().ToLower()) != "end")
            {
                if (userChoice == "products")
                {
                    Console.WriteLine(UserMessages.SelectCategory);

                    string category = Console.ReadLine().ToLower();

                    while (!CheckIfCategoryIsValid(category))
                    {
                        Console.WriteLine(ErrorMessages.EnterValidCategory);
                        category = Console.ReadLine().ToLower();
                    }

                    DisplayCategory(category, configuration);
                    Console.WriteLine(UserMessages.UserChoices);
                }
                else if (userChoice == "create")
                {
                    Console.Write(UserMessages.ChooseParts);
                    string[] input = Console.ReadLine().Split(", ").ToArray();

                    Console.WriteLine(string.Join(' ', input));
                }
                else
                {
                    Console.WriteLine(ErrorMessages.EnterValidCommand);
                }
            }
        }

        private static bool CheckIfCategoryIsValid(string input)
        {
            if (input is "cpu" or "memory" or "motherboard")
            {
                return true;
            }

            return false;
        }

        private static void DisplayCategory(string category, Configuration configuration)
        {
            switch (category)
            {
                case "cpu":
                    Console.WriteLine(string.Join(' ', configuration.CPUs));
                    break;
                case "memory":
                    Console.WriteLine(string.Join(' ', configuration.Memory));
                    break;
                case "motherboard":
                    Console.WriteLine(string.Join(' ', configuration.Motherboards));
                    break;
            }
        }
    }
}