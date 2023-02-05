namespace PCConfigurator.Configurations;

using Models;
using Utilities;

public class ValidateConfiguration
{
    // Displaying all possible configurations when only 1 param is entered.
    public static void ValidateConfigurationWithOneParam
        (string firstPart, ConfigurationData configuration, HashSet<CreateConfiguration> possibleConfigurations)
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
            Console.WriteLine(UserMessages.NumberOfCombination, index++);
            Console.WriteLine(string.Join(' ', config.ToString()));
        }

        Console.WriteLine(UserMessages.UserChoices);
    }
    // Displaying all possible configurations when two params are entered.
    public static void ValidateConfigurationWithTwoParams
        (string firstPart, string secondPart, ConfigurationData configuration, HashSet<CreateConfiguration> possibleConfigurations)
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
                Console.WriteLine(ErrorMessages.IncompatibleMotherboard, motherboard.Socket);
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
                Console.WriteLine(ErrorMessages.IncompatibleMemory, memory.Type);
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
                Console.WriteLine(ErrorMessages.IncompatibleMotherboard, motherboard.Socket);
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
                Console.WriteLine(ErrorMessages.IncompatibleMemory, memory.Type);
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
            Console.WriteLine(UserMessages.NumberOfCombination, index++);
            Console.WriteLine(string.Join(' ', config.ToString()));
        }

        Console.WriteLine(UserMessages.UserChoices);
    }
    // Checking if the configuration with all params entered is valid
    public static void ValidateConfigurationWithThreeParams
        (string firstPart, string secondPart, string thirdPart, ConfigurationData configuration)
    {
        string errors = string.Empty;
        CPU cpu = new();
        Motherboard motherboard = new();
        Memory memory = new();

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
            Console.WriteLine(ErrorMessages.ConfigurationNotValid);
            Console.WriteLine(errors);
        }

        Console.WriteLine(UserMessages.UserChoices);
    }
}