namespace PCConfigurator;

using Models;
using Utilities;
using System.Text;

public class CreateConfiguration
{
    public CreateConfiguration(CPU cpu, Motherboard motherboard, Memory memory)
    {
        this.CPU = cpu;
        this.Memory = memory;
        this.Motherboard = motherboard;
    }
    public CPU CPU { get; }
    public Memory Memory { get; }
    public Motherboard Motherboard { get; }

    public decimal TotalPrice => CPU.Price + Memory.Price + Motherboard.Price;

    // Check if configuration is valid, if not returns error. 
    public static string ValidateConfiguration(CPU cpu, Motherboard motherboard, Memory memory)
    {
        StringBuilder sb = new();

        if (cpu.Socket != motherboard.Socket)
        {
            sb.AppendLine(string.Format(ErrorMessages.IncompatibleMotherboard, motherboard.Socket));
        }

        if (cpu.SupportedMemory != memory.Type)
        {
            sb.AppendLine(string.Format(ErrorMessages.IncompatibleMemory, memory.Type));
        }

        return sb.ToString();
    }

    // Getting all possible configurations with only CPU entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
        (CPU cpu, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<Memory> compatibleMemory =
            configuration.Memory.Where(m => m.Type == cpu.SupportedMemory);
        IEnumerable<Motherboard> compatibleMotherboard =
            configuration.Motherboards.Where(m => m.Socket == cpu.Socket);

        foreach (Memory memory in compatibleMemory)
        {
            foreach (Motherboard motherboard in compatibleMotherboard)
            {
                possibleConfigurations.Add(new CreateConfiguration(cpu, motherboard, memory));
            }
        }

        return possibleConfigurations;
    }

    // Getting all possible configurations with only Motherboard entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
        (Motherboard motherboard, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<CPU> compatibleCPUs =
            configuration.CPUs.Where(c => c.Socket == motherboard.Socket);
        IEnumerable<Memory> compatibleMemory =
            configuration.Memory.Where(m => m.Type == compatibleCPUs.First().SupportedMemory);

        foreach (CPU cpu in compatibleCPUs)
        {
            foreach (Memory memory in compatibleMemory)
            {
                CreateConfiguration config = new CreateConfiguration(cpu, motherboard, memory);
                possibleConfigurations.Add(config);
            }
        }

        return possibleConfigurations;
    }

    // Getting all possible configurations with only Memory entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
        (Memory memory, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<CPU> compatibleCPUs =
            configuration.CPUs.Where(c => c.SupportedMemory == memory.Type);
        List<Motherboard> compatibleMotherboards = new();

        foreach (CPU cpu in compatibleCPUs)
        {
            HashSet<Motherboard> motherboard
                = configuration.Motherboards.Where(m => m.Socket == cpu.Socket).ToHashSet();
            compatibleMotherboards.AddRange(motherboard);
        }

        HashSet<Motherboard> motherboards = compatibleMotherboards.ToHashSet();

        foreach (CPU cpu in compatibleCPUs)
        {
            foreach (Motherboard motherboard in motherboards.Where(m => m.Socket == cpu.Socket))
            {
                possibleConfigurations.Add(new CreateConfiguration(cpu, motherboard, memory));
            }
        }

        return possibleConfigurations;
    }

    // Getting all possible configurations with CPU and Motherboard entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
    (CPU cpu, Motherboard motherboard, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<Memory> compatibleMemory =
            configuration.Memory.Where(m => m.Type == cpu.SupportedMemory);

        foreach (Memory memory in compatibleMemory)
        {
            possibleConfigurations.Add(new CreateConfiguration(cpu, motherboard, memory));
        }

        return possibleConfigurations;
    }

    // Getting all possible configurations with CPU and Memory entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
        (CPU cpu, Memory memory, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<Motherboard> compatibleMotherboards =
            configuration.Motherboards.Where(m => m.Socket == cpu.Socket);

        foreach (Motherboard motherboard in compatibleMotherboards)
        {
            possibleConfigurations.Add(new CreateConfiguration(cpu, motherboard, memory));
        }

        return possibleConfigurations;
    }

    // Getting all possible configurations with Motherboard and Memory entered.
    public static HashSet<CreateConfiguration> GetAllConfigurations
        (Motherboard motherboard, Memory memory, Configuration configuration, HashSet<CreateConfiguration> possibleConfigurations)
    {
        IEnumerable<CPU> compatibleCPUs =
            configuration.CPUs.Where(c => c.Socket == motherboard.Socket && c.SupportedMemory == memory.Type);

        foreach (CPU cpu in compatibleCPUs)
        {
            possibleConfigurations.Add(new CreateConfiguration(cpu, motherboard, memory));
        }

        return possibleConfigurations;
    }
    public override string ToString()
    {
        return $"{Environment.NewLine}" +
               $"CPU: {CPU.Name} - {CPU.Socket}, {CPU.SupportedMemory}{Environment.NewLine}" +
               $"Motherboard: {Motherboard.Name} - {Motherboard.Socket}{Environment.NewLine}" +
               $"Memory: {Memory.Name} - {Memory.Type}{Environment.NewLine}" +
               $"Price: {this.TotalPrice:f0}${Environment.NewLine}" +
               $"{Environment.NewLine}" +
               "***********" +
               $"{Environment.NewLine}";
    }
}