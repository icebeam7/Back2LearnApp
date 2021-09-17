namespace Back2LearnApp.Models
{
    public class Country
    {
        public int GeonameId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public string Capital { get; set; }
        public string CurrencyCode { get; set; }
        public string ContinentName { get; set; }
        public int Population { get; set; }
    }
}
