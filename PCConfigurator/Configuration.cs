namespace PCConfigurator;

using Models;

public class Configuration
{
    public IEnumerable<CPU> CPUs { get; set; }
    public IEnumerable<Memory> Memory { get; set; }
    public IEnumerable<Motherboard> Motherboards { get; set; }
}