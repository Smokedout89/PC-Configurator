namespace PCConfigurator.Models
{
    using System.Text;

    public abstract class Common
    {
        public string ComponentType { get; set; }
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Environment.NewLine}" +
                   $"Component type: {this.ComponentType}{Environment.NewLine}" +
                   $"Part number: {this.PartNumber}{Environment.NewLine}" +
                   $"Name: {this.Name}{Environment.NewLine}" +
                   $"Price: {this.Price}" +
                   $"{Environment.NewLine}";
        }
    }
}
