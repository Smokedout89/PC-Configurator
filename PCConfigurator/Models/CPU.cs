namespace PCConfigurator.Models
{
    public class CPU : Common
    {
        public string SupportedMemory { get; }
        public string Socket { get; }
        public override string ToString()
            => base.ToString() + $" with a socket {this.Socket} and supports {this.SupportedMemory}";
    }
}
