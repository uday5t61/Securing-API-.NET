namespace WebApp_UnderTheHood.DTO
{
    public class WeatherForcast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
    }
}
