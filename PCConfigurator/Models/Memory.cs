namespace PCConfigurator.Models
{
    public class Memory : Common
    {
        public string Type { get; }

        public override string ToString()
            => base.ToString() + $" and is type {this.Type}";
    }
}
