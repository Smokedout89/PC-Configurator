namespace PCConfigurator
{
    using System;
    using Utilities;
    using Configurations;
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
            HashSet<CreateConfiguration> possibleConfigurations = new();

                                    // Reading and checking what inputs the user provided.
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
                    string[] componentsInput = Console.ReadLine()
                            .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                            .ToArray();

                    for (int i = 0; i < componentsInput.Length; i++)
                    {
                        componentsInput[i] = componentsInput[i].Trim();
                    }

                    ValidateComponentsInput(componentsInput, configuration, possibleConfigurations);
                }
                else
                {
                    Console.WriteLine(ErrorMessages.EnterValidCommand);
                }
            }
        }
                                // Validating the input if the user is creating a configuration
        private static void ValidateComponentsInput
            (string[] componentsInput, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
        {
            possibleConfigurations = new();

            if (componentsInput.Length == 0)
            {
                Console.WriteLine(ErrorMessages.NoComponents);
                Console.WriteLine(UserMessages.UserChoices);
            }

            if (componentsInput.Length > 3)
            {
                Console.WriteLine(ErrorMessages.TooManyComponents);
                Console.WriteLine(UserMessages.UserChoices);
            }

            if (componentsInput.Length == 1)
            {
                string firstPart = componentsInput[0].ToLower();
                ValidateConfiguration.ValidateConfigurationWithOneParam(firstPart, configuration, possibleConfigurations);
            }
            else if (componentsInput.Length == 2)
            {
                string firstPart = componentsInput[0].ToLower();
                string secondPart = componentsInput[1].ToLower();
                ValidateConfiguration.ValidateConfigurationWithTwoParams(firstPart, secondPart, configuration, possibleConfigurations);
            }
            else if (componentsInput.Length == 3)
            {
                string firstPart = componentsInput[0].ToLower();
                string secondPart = componentsInput[1].ToLower();
                string thirdPart = componentsInput[2].ToLower();
                ValidateConfiguration.ValidateConfigurationWithThreeParams(firstPart, secondPart, thirdPart, configuration);
            }
        }
                                // Displaying all the available parts for the user selected category.
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

        private static bool CheckIfCategoryIsValid(string input)
        {
            return input is "cpu" or "memory" or "motherboard";
        }
    }
}