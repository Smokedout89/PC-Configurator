namespace PCConfigurator.Models
{
    public abstract class Common
    {
        public string ComponentType { get; }
        public string PartNumber { get; }
        public string Name { get; }
        public decimal Price { get; }

        public override string ToString()
            => $"This {this.ComponentType} with name {this.Name} and part number {this.PartNumber} costs {this.Price}";
    }
}
