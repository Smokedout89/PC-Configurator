namespace PCConfigurator.Models
{
    public class CPU : Common
    {
        public string SupportedMemory { get; set; }
        public string Socket { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                   $"Supported memory: {this.SupportedMemory}{Environment.NewLine}" +
                   $"Socket: {this.Socket}{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   "***********" +
                   $"{Environment.NewLine}";
        }
    }
}