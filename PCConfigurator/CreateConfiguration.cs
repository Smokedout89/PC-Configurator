namespace PCConfigurator;

using Models;

public class CreateConfiguration
{
    public CreateConfiguration(CPU cpu, Memory memory, Motherboard motherboard)
    {
        this.CPU = cpu;
        this.Memory = memory;
        this.Motherboard = motherboard;
    }
    public CPU CPU { get; private set; }
    public Memory Memory { get; private set; }
    public Motherboard Motherboard { get; private set; }

    public decimal TotalPrice => CPU.Price + Memory.Price + Motherboard.Price;



    public override string ToString()
    {
        return $"CPU: {CPU.Name}{Environment.NewLine}" +
               $"Memory: {Memory.Name}{Environment.NewLine}" +
               $"Motherboard: {Motherboard.Name}{Environment.NewLine}" +
               $"Price: {this.TotalPrice:f2}{Environment.NewLine}" +
               $"{Environment.NewLine}" +
               "***********";
    }
}