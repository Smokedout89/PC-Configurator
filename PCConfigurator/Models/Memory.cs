namespace PCConfigurator.Models
{
    using System.Text;

    public class Memory : Common
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                   $"Type: {this.Type}{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   "***********" +
                   $"{Environment.NewLine}";
        }
    }
}
