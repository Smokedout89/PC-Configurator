namespace PCConfigurator
{
    using System;
    using Models;
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
            HashSet<CreateConfiguration> possibleConfigurations = new();

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
                ValidateConfigurationWithOneParam(firstPart, configuration, possibleConfigurations);
            }
            else if (componentsInput.Length == 2)
            {
                string firstPart = componentsInput[0].ToLower();
                string secondPart = componentsInput[1].ToLower();
                ValidateConfigurationWithTwoParams(firstPart, secondPart, configuration, possibleConfigurations);
            }
            else if (componentsInput.Length == 3)
            {
                string firstPart = componentsInput[0].ToLower();
                string secondPart = componentsInput[1].ToLower();
                string thirdPart = componentsInput[2].ToLower();
                ValidateConfigurationWithThreeParams(firstPart, secondPart, thirdPart, configuration);
            }
        }

        private static void ValidateConfigurationWithOneParam
            (string firstPart, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
        {
            int index = 1;
            HashSet<CreateConfiguration> configurations = new HashSet<CreateConfiguration>();

            if (configuration.CPUs.Any(p => p.PartNumber.ToLower() == firstPart))
            {
                CPU cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == firstPart);

                configurations =
                    CreateConfiguration.GetAllConfigurations(cpu, configuration, possibleConfigurations);
            }
            else if (configuration.Motherboards.Any(p => p.PartNumber.ToLower() == firstPart))
            {
                Motherboard motherboard =
                    configuration.Motherboards.First(p => p.PartNumber.ToLower() == firstPart);

                configurations =
                    CreateConfiguration.GetAllConfigurations(motherboard, configuration, possibleConfigurations);
            }
            else if (configuration.Memory.Any(p => p.PartNumber.ToLower() == firstPart))
            {
                Memory memory = configuration.Memory.First(p => p.PartNumber.ToLower() == firstPart);

                configurations =
                    CreateConfiguration.GetAllConfigurations(memory, configuration, possibleConfigurations);
            }
            else
            {
                Console.WriteLine(ErrorMessages.InvalidPartNumber);
                Console.WriteLine(UserMessages.UserChoices);
                return;
            }

            Console.WriteLine(string.Format(UserMessages.PossibleConfigurations, configurations.Count)
                              + $"{Environment.NewLine}");

            foreach (var config in configurations.OrderByDescending(p => p.TotalPrice))
            {
                Console.WriteLine(string.Format(UserMessages.NumberOfCombination, index++));
                Console.WriteLine(string.Join(' ', config.ToString()));
            }

            Console.WriteLine(UserMessages.UserChoices);
        }

        private static void ValidateConfigurationWithTwoParams
            (string firstPart, string secondPart, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
        {
            int index = 1;
            HashSet<CreateConfiguration> configurations = new HashSet<CreateConfiguration>();

            if (configuration.CPUs.Any(p => p.PartNumber.ToLower() == firstPart)
                && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                CPU cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == firstPart);
                Motherboard motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == secondPart);

                if (cpu.Socket != motherboard.Socket)
                {
                    Console.WriteLine(string.Format(ErrorMessages.IncompatibleMotherboard, motherboard.Socket));
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(cpu, motherboard, configuration, possibleConfigurations);
            }
            else if (configuration.CPUs.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Memory.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                CPU cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == firstPart);
                Memory memory = configuration.Memory.First(p => p.PartNumber.ToLower() == secondPart);

                if (cpu.SupportedMemory != memory.Type)
                {
                    Console.WriteLine(string.Format(ErrorMessages.IncompatibleMemory, memory.Type));
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(cpu, memory, configuration, possibleConfigurations);
            }
            else if (configuration.Motherboards.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                Motherboard motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == firstPart);
                CPU cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == secondPart);

                if (cpu.Socket != motherboard.Socket)
                {
                    Console.WriteLine(string.Format(ErrorMessages.IncompatibleMotherboard, motherboard.Socket));
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(cpu, motherboard, configuration, possibleConfigurations);
            }
            else if (configuration.Motherboards.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Memory.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                Motherboard motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == firstPart);
                Memory memory = configuration.Memory.First(p => p.PartNumber.ToLower() == secondPart);

                IEnumerable<CPU> possibleCPU = configuration.CPUs.Where(c => c.Socket == motherboard.Socket);
                List<CPU> cpus = possibleCPU.Where(cpu => memory.Type == cpu.SupportedMemory).ToList();

                if (cpus.Count == 0)
                {
                    Console.WriteLine(ErrorMessages.IncompatibleParts);
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(motherboard, memory, configuration, possibleConfigurations);
            }
            else if (configuration.Memory.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                Memory memory = configuration.Memory.First(p => p.PartNumber.ToLower() == firstPart);
                CPU cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == secondPart);

                if (cpu.SupportedMemory != memory.Type)
                {
                    Console.WriteLine(string.Format(ErrorMessages.IncompatibleMemory, memory.Type));
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(cpu, memory, configuration, possibleConfigurations);
            }
            else if (configuration.Memory.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == secondPart))
            {
                Memory memory = configuration.Memory.First(p => p.PartNumber.ToLower() == firstPart);
                Motherboard motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == secondPart);

                IEnumerable<CPU> possibleCPU = configuration.CPUs.Where(c => c.Socket == motherboard.Socket);
                List<CPU> cpus = possibleCPU.Where(cpu => memory.Type == cpu.SupportedMemory).ToList();

                if (cpus.Count == 0)
                {
                    Console.WriteLine(ErrorMessages.IncompatibleParts);
                    Console.WriteLine(UserMessages.UserChoices);
                    return;
                }

                configurations =
                    CreateConfiguration.GetAllConfigurations(motherboard, memory, configuration, possibleConfigurations);
            }
            else
            {
                Console.WriteLine(ErrorMessages.InvalidPartNumber);
                Console.WriteLine(UserMessages.UserChoices);
                return;
            }

            Console.WriteLine(string.Format(UserMessages.PossibleConfigurations, configurations.Count) +
                              $"{Environment.NewLine}");

            foreach (var config in configurations.OrderByDescending(p => p.TotalPrice))
            {
                Console.WriteLine(string.Format(UserMessages.NumberOfCombination, index++));
                Console.WriteLine(string.Join(' ', config.ToString()));
            }

            Console.WriteLine(UserMessages.UserChoices);
        }

        private static void ValidateConfigurationWithThreeParams
            (string firstPart, string secondPart, string thirdPart, Configuration configuration)
        {
            string errors = String.Empty;
            CPU cpu = new CPU();
            Motherboard motherboard = new Motherboard();
            Memory memory = new Memory();

            if (configuration.CPUs.Any(p => p.PartNumber.ToLower() == firstPart)
                && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == secondPart)
                && configuration.Memory.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == firstPart);
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == secondPart);
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }
            else if (configuration.CPUs.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Memory.Any(p => p.PartNumber.ToLower() == secondPart)
                     && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == firstPart);
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == secondPart);
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }
            else if (configuration.Motherboards.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == secondPart)
                     && configuration.Memory.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == firstPart);
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == secondPart);
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }
            else if (configuration.Motherboards.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Memory.Any(p => p.PartNumber.ToLower() == secondPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == firstPart);
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == secondPart);
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }
            else if (configuration.Memory.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == secondPart)
                     && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == firstPart);
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == secondPart);
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }
            else if (configuration.Memory.Any(p => p.PartNumber.ToLower() == firstPart)
                     && configuration.Motherboards.Any(p => p.PartNumber.ToLower() == secondPart)
                     && configuration.CPUs.Any(p => p.PartNumber.ToLower() == thirdPart))
            {
                memory = configuration.Memory.First(p => p.PartNumber.ToLower() == firstPart);
                motherboard = configuration.Motherboards.First(p => p.PartNumber.ToLower() == secondPart);
                cpu = configuration.CPUs.First(p => p.PartNumber.ToLower() == thirdPart);

                errors = CreateConfiguration.ValidateConfiguration(cpu, motherboard, memory);
            }

            if (errors == string.Empty)
            {
                CreateConfiguration config = new CreateConfiguration(cpu, motherboard, memory);
                Console.WriteLine(config.ToString());
            }
            else
            {
                Console.WriteLine(errors);
            }

            Console.WriteLine(UserMessages.UserChoices);
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

        private static bool CheckIfCategoryIsValid(string input)
        {
            return input is "cpu" or "memory" or "motherboard";
        }
    }
}