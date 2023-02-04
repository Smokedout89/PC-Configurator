namespace PCConfigurator
{
    using System.Text.Json;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Read data from json file and deserialize it.
            var jsonData = File.ReadAllText("../../../JsonData/pc-store-inventory.json");
            Configuration configuration = JsonSerializer.Deserialize<Configuration>(jsonData);

            Console.WriteLine(string.Join(' ', configuration.CPUs).TrimStart());
            Console.WriteLine(string.Join(' ', configuration.Memory).TrimStart());
            Console.WriteLine(string.Join(' ', configuration.Motherboards).TrimStart());
        }
    }
}