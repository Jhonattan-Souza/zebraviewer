namespace ZebraViewer.Models
{
    public class Printer
    {
        public string Name { get; set; }
        public string PortName { get; set; }
        public string OldPortName { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
