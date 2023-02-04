namespace PCConfigurator
{
    using Utilities;
    using System.Text.Json;

    internal class StartUp
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

                    var cpu = configuration.CPUs.FirstOrDefault(c => c.PartNumber == input[0]);
                    var motherboard = configuration.Motherboards.FirstOrDefault(m => m.PartNumber == input[1]);
                    var memory = configuration.Memory.FirstOrDefault(m => m.PartNumber == input[2]);
                    

                    Console.WriteLine(CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory));
                }
                else
                {
                    Console.WriteLine(ErrorMessages.EnterValidCommand);
                }
            }
        }

        private static bool CheckIfCategoryIsValid(string input)
        {
            return input is "cpu" or "memory" or "motherboard";
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