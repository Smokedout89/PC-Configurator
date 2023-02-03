namespace PCConfigurator.Models
{
    public class Motherboard : Common
    {
        public string Socket { get; }

        public override string ToString()
            => base.ToString() + $" with a socket {this.Socket}";
    }
}
