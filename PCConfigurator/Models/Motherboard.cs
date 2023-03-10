namespace PCConfigurator.Models
{
    public class Motherboard : Common
    {
        public string Socket { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                   $"Socket: {this.Socket}{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   "***********" +
                   $"{Environment.NewLine}";
        }
    }
}
