namespace Weather.API.Models
{
    public class CityInfo
    {
        public List<Infos> Infos { get; set; } = new List<Infos>();
    }

    public class Infos
    {
        public string Name { get; set; } = "";
        public LocalNames? LocalNames { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Country { get; set; } = "";
        public string State { get; set; } = "";
    }

    public class LocalNames
    {
        public string Pt { get; set; } = "";
    }
}
